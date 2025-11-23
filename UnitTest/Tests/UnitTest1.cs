using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using LW4._2_Kovalchuk.Enum;
using LW4._2_Kovalchuk.Interfaces;
using LW4._2_Kovalchuk.Models;
using LW4._2_Kovalchuk.Services;
using Moq;
using Xunit;

namespace LW4._2_Kovalchuk.Tests
{
    public class UsersServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly Mock<IValidator<UserItem>> _mockValidator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UsersService _service;

        public UsersServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockValidator = new Mock<IValidator<UserItem>>();
            _mockMapper = new Mock<IMapper>();
            _service = new UsersService(_mockRepository.Object, _mockValidator.Object, _mockMapper.Object);
        }

        [Fact]
        public void ToDto_ValidUser_ReturnsMappedDto()
        {
            var user = new UserItem { Id = 1, Email = "test@example.com" };
            var dto = new UserDTO { Id = 1, Email = "test@example.com" };
            _mockMapper.Setup(m => m.Map<UserDTO>(user)).Returns(dto);

            var result = _service.ToDto(user);

            Assert.Equal(dto, result);
            _mockMapper.Verify(m => m.Map<UserDTO>(user), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ValidUser_CreatesAndReturnsUser()
        {
            var user = new UserItem { Id = 1, Email = "test@example.com", Role = Roles.User };
            _mockValidator.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new ValidationResult());
            _mockRepository.Setup(r => r.CreateAsync(user)).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(user);

            Assert.Equal(user, result);
            _mockRepository.Verify(r => r.CreateAsync(user), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_InvalidUser_ThrowsValidationException()
        {
            var user = new UserItem();
            var errors = new List<ValidationFailure> { new("Email", "Invalid email") };
            _mockValidator.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new ValidationResult(errors));

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _service.CreateAsync(user));
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<UserItem>()), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            var users = new List<UserItem>
            {
                new() { Id = 1, Role = Roles.User },
                new() { Id = 2, Role = Roles.Admin }
            };
            _mockRepository.Setup(r => r.GetAsync()).ReturnsAsync(users);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal(users, result);
        }

        [Fact]
        public async Task GetAllAsync_EmptyRepository_ReturnsEmptyList()
        {
            _mockRepository.Setup(r => r.GetAsync()).ReturnsAsync(new List<UserItem>());

            var result = await _service.GetAllAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsUser()
        {
            var user = new UserItem { Id = 1, Role = Roles.User };
            _mockRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(user);

            var result = await _service.GetByIdAsync(1);

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            _mockRepository.Setup(r => r.GetAsync(999)).ReturnsAsync((UserItem)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(999));
        }

        [Fact]
        public async Task GetByEmailAsync_ExistingEmail_ReturnsUser()
        {
            var user = new UserItem { Id = 1, Email = "test@example.com", Role = Roles.User };
            _mockRepository.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(user);

            var result = await _service.GetByEmailAsync("test@example.com");

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetByEmailAsync_NonExistingEmail_ReturnsNull()
        {
            _mockRepository.Setup(r => r.GetByEmailAsync("nonexistent@example.com")).ReturnsAsync((UserItem)null);

            var result = await _service.GetByEmailAsync("nonexistent@example.com");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_EmptyEmail_ReturnsNull()
        {
            _mockRepository.Setup(r => r.GetByEmailAsync("")).ReturnsAsync((UserItem)null);

            var result = await _service.GetByEmailAsync("");

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ValidUserAndExistingId_UpdatesUser()
        {
            var existingUser = new UserItem { Id = 1, Role = Roles.User };
            var updatedUser = new UserItem { Email = "updated@example.com", Role = Roles.Admin };
            _mockRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(existingUser);
            _mockValidator.Setup(v => v.ValidateAsync(updatedUser, default)).ReturnsAsync(new ValidationResult());
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<UserItem>())).Returns(Task.CompletedTask);

            await _service.UpdateAsync(1, updatedUser);

            _mockRepository.Verify(r => r.UpdateAsync(It.Is<UserItem>(u => u.Id == 1 && u.Email == "updated@example.com" && u.Role == Roles.Admin)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidUser_ThrowsValidationException()
        {
            var existingUser = new UserItem { Id = 1 };
            var updatedUser = new UserItem();
            _mockRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(existingUser);
            var errors = new List<ValidationFailure> { new("Email", "Invalid email") };
            _mockValidator.Setup(v => v.ValidateAsync(updatedUser, default)).ReturnsAsync(new ValidationResult(errors));

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _service.UpdateAsync(1, updatedUser));
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<UserItem>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            _mockRepository.Setup(r => r.GetAsync(999)).ReturnsAsync((UserItem)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(999, new UserItem()));
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_DeletesUser()
        {
            var user = new UserItem { Id = 1, Role = Roles.User };
            _mockRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(1);

            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            _mockRepository.Setup(r => r.GetAsync(999)).ReturnsAsync((UserItem)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(999));
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(Roles.User, 2)]
        [InlineData(Roles.Admin, 1)]
        [InlineData(Roles.Manager, 1)]
        public async Task GetCountByRoleAsync_ValidRole_ReturnsCorrectCount(Roles role, int expectedCount)
        {
            var users = new List<UserItem>
            {
                new() { Role = Roles.User },
                new() { Role = Roles.User | Roles.Manager },
                new() { Role = Roles.Admin }
            };
            _mockRepository.Setup(r => r.GetAsync()).ReturnsAsync(users);

            var result = await _service.GetCountByRoleAsync(role);

            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public async Task GetCountByRoleAsync_NoneRole_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetCountByRoleAsync(Roles.None));
        }

        [Fact]
        public async Task GetCountByRoleAsync_EmptyUsersList_ReturnsZero()
        {
            _mockRepository.Setup(r => r.GetAsync()).ReturnsAsync(new List<UserItem>());

            var result = await _service.GetCountByRoleAsync(Roles.User);

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetCountByRoleAsync_CombinedRoles_CountsCorrectly()
        {
            var users = new List<UserItem>
            {
                new() { Role = Roles.User | Roles.Admin },
                new() { Role = Roles.Admin },
                new() { Role = Roles.User }
            };
            _mockRepository.Setup(r => r.GetAsync()).ReturnsAsync(users);

            var result = await _service.GetCountByRoleAsync(Roles.Admin);

            Assert.Equal(2, result);
        }
    }
}