using Xunit;
using Gierka.Core;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Gierka.Tests
{
    public class AuthServiceComponentTests : IDisposable
    {
        private readonly string _dbPath;

        public AuthServiceComponentTests()
        {
            // Set up a clean database for this test class execution
            // Note: Since GameService and AuthService use 'new GierkaDbContext()', they will look for 'gierka.db' in the bin directory.
            // We can't easily isolate per-test unless we change the code to support injection.
            // But for this environment, we will assume sequential execution or just use unique data.
            _dbPath = Path.Combine(AppContext.BaseDirectory, "gierka.db");
            
            // Ensure DB is created
            using (var context = new GierkaDbContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public void Dispose()
        {
            // Helper to clean up if needed, but deleting DB might affect other parallel tests.
            // Leaving it effectively empty for now, relying on unique user data.
        }

        [Fact]
        public void Register_ShouldCreateUser_WhenDataisValid()
        {
            // Arrange
            var authService = new AuthService();
            string login = "testuser_" + Guid.NewGuid().ToString().Substring(0, 8);
            string password = "password123";

            // Act
            bool result = authService.Register(login, password);

            // Assert
            Assert.True(result);
            
            // Verify in DB
            using (var context = new GierkaDbContext())
            {
                var user = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(context.Users, u => u.Login == login).Result;
                Assert.NotNull(user);
            }
        }

        [Fact]
        public void Login_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            // Arrange
            var authService = new AuthService();
            string login = "loginuser_" + Guid.NewGuid().ToString().Substring(0, 8);
            string password = "password123";
            authService.Register(login, password);

            // Act
            var user = authService.Login(login, password);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(login, user.Login);
        }

        [Fact]
        public void Login_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var authService = new AuthService();
            string login = "wrongpass_" + Guid.NewGuid().ToString().Substring(0, 8);
            string password = "password123";
            authService.Register(login, password);

            // Act
            var user = authService.Login(login, "wrong");

            // Assert
            Assert.Null(user);
        }
    }
}
