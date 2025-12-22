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
            int questionID = 1;
            string questionText = "This is a question";
            string questionCorrectAnswer = "A";
            string questionDifficulty = "Easy";
            string questionOptions = "A, B, C";

            // Act
            Question test = new Question(questionID, questionText, questionOptions, questionCorrectAnswer, questionDifficulty);

            // Assert
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
            Question test = new Question(1, "This is a question", "A", "Easy", "A, B, C");
            String newQuestionText = "Is this a question";

            // Act
            test.QuestionText = newQuestionText;

            // Assert
            Assert.AreEqual(newQuestionText, test.QuestionText);
        }
    }
}
