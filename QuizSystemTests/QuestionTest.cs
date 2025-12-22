using static System.Net.Mime.MediaTypeNames;

namespace QuestionTests
{
    [TestClass]
    public class QuestionTest
    {
        [TestMethod]
        public void QuestionConstructor_ShouldInitialiseProperties()
        {
            // Arrange
            // Add known values to pass into contructor
            int questionID = 1;
            string questionText = "This is a question";
            string questionCorrectAnswer = "A";
            string questionDifficulty = "Easy";
            string questionOptions = "A, B, C";

            // Act
            // Create a new Question object using the arranged values
            Question test = new Question(questionID, questionText, questionOptions, questionCorrectAnswer, questionDifficulty);

            // Assert
            // Verify that contructor correctly assigned values to properties
            Assert.AreEqual(questionID, test.QuestionID);
            Assert.AreEqual(questionText, test.QuestionText);
            Assert.AreEqual(questionCorrectAnswer, test.QuestionCorrectAnswer);
            Assert.AreEqual(questionDifficulty, test.QuestionDifficulty);
            Assert.AreEqual(questionOptions, test.QuestionOptions);
        }

        [TestMethod]
        public void TextProperty_ShouldGetAndSet()
        {
            // Arrange
            // Create a question object with initial values
            Question test = new Question(1, "This is a question", "A", "Easy", "A, B, C");
            String newQuestionText = "Is this a question";

            // Act
            // Update the QuestionText property
            test.QuestionText = newQuestionText;

            // Assert
            // Confirm the property was updated correctly
            Assert.AreEqual(newQuestionText, test.QuestionText);
        }
    }
}
