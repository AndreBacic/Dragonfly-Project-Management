using DragonflyDataLibrary.Security;
using Xunit;

namespace Dragonfly.Tests
{
    public class HashAndSalterTests
    {
        [Fact]
        public void HashAndSalt_ShouldWork()
        {
            // Arrange
            string password = "Test";
            // Act
            PasswordHashModel model = HashAndSalter.HashAndSalt(password);
            // Assert
            Assert.NotNull(model);
            Assert.True(model.PasswordHashString.Length == 44);
            Assert.Equal(HashAndSalter.Iterations, model.IterationsOnHash);
        }
        [Fact]
        public void HashAndSalt_TestWeirdInputs()
        {
            // Arrange
            string password = "";
            // Act
            PasswordHashModel model = HashAndSalter.HashAndSalt(password);
            // Assert
            Assert.NotNull(model);
            Assert.True(model.PasswordHashString.Length == 44);
        }
        [Fact]
        public void PasswordEqualsHash_ShouldWork()
        {
            // Arrange
            string password1 = "Pass123";
            string password2 = "Pass321";
            // Act
            PasswordHashModel model1 = HashAndSalter.HashAndSalt(password1);

            (bool password1IsHash, bool needsUpgrade1) = HashAndSalter.PasswordEqualsHash(password1, model1);
            (bool password2IsHash, bool needsUpgrade2) = HashAndSalter.PasswordEqualsHash(password2, model1);
            // Assert
            Assert.True(password1IsHash);
            Assert.False(password2IsHash);
            Assert.False(needsUpgrade1);
        }
    }
}
