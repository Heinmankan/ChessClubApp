using NUnit.Framework;

namespace ChessClub.Service.Tests
{
    [TestFixture]
    public partial class ChessClubServiceTests
    {
        [Test]
        public void TestUpdateMember()
        {
            var updateMember = _chessClubContext?.Members.First();

            Assert.IsNotNull(updateMember);

            var id = updateMember.Id;
            var name = updateMember.Name;
            var surname = updateMember.Surname;
            var email = updateMember.Email;
            var birthday = updateMember.Birthday;

            var result = _chessClubService?.UpdateMember(id, name + "Test", surname + "Test", "Test" + email, birthday.AddDays(-10));

            Assert.AreEqual(true, result, "Unexpected 'Update' result");

            var updatedMember = _chessClubContext?.Members.First(m => m.Id == id);

            Assert.AreEqual(name + "Test", updatedMember?.Name);
            Assert.AreEqual(surname + "Test", updatedMember?.Surname);
            Assert.AreEqual("Test" + email, updatedMember?.Email);
            Assert.AreEqual(birthday.AddDays(-10), updatedMember?.Birthday);
        }

        [Test]
        public void TestUpdateMemberNoChanges()
        {
            var updateMember = _chessClubContext?.Members.First();

            Assert.IsNotNull(updateMember);

            var result = _chessClubService?.UpdateMember(updateMember.Id);

            Assert.AreEqual(true, result, "Unexpected 'Update' result");

            var updatedMember = _chessClubContext?.Members.First(m => m.Id == updateMember.Id);

            Assert.AreEqual(updateMember.Name, updatedMember?.Name);
            Assert.AreEqual(updateMember.Surname, updatedMember?.Surname);
            Assert.AreEqual(updateMember.Email, updatedMember?.Email);
            Assert.AreEqual(updateMember.Birthday, updatedMember?.Birthday);
        }

        [Test]
        [TestCase("NewName", "", "")]
        [TestCase("", "NewSurname", "")]
        [TestCase("", "", "NewEmail@gmail.com")]
        public void TestUpdateMemberSingleItemOnly(string name, string surname, string email)
        {
            var updateMember = _chessClubContext?.Members.First();

            Assert.IsNotNull(updateMember);

            var id = updateMember.Id;

            var testName = string.IsNullOrWhiteSpace(name) ? string.Empty : name;
            var testSurname = string.IsNullOrWhiteSpace(surname) ? string.Empty : surname;
            var testEmail = string.IsNullOrWhiteSpace(email) ? string.Empty : email;

            var result = _chessClubService?.UpdateMember(updateMember.Id, testName, testSurname, testEmail);

            Assert.AreEqual(true, result, "Unexpected 'Update' result");

            var updatedMember = _chessClubContext?.Members.First(m => m.Id == id);

            var expectedName = string.IsNullOrWhiteSpace(name) ? updateMember.Name : name;
            var expectedSurname = string.IsNullOrWhiteSpace(surname) ? updateMember.Surname : surname;
            var expectedEmail = string.IsNullOrWhiteSpace(email) ? updateMember.Email : email;

            Assert.AreEqual(expectedName, updatedMember?.Name);
            Assert.AreEqual(expectedSurname, updatedMember?.Surname);
            Assert.AreEqual(expectedEmail, updatedMember?.Email);
            Assert.AreEqual(updateMember.Birthday, updatedMember?.Birthday);
        }

        [Test]
        public void TestUpdateMemberBirthdayOnly()
        {
            var updateMember = _chessClubContext?.Members.First();

            Assert.IsNotNull(updateMember);

            var id = updateMember.Id;
            var birthday = updateMember.Birthday.AddDays(-10);

            var result = _chessClubService?.UpdateMember(updateMember.Id, birthday: birthday);

            Assert.AreEqual(true, result, "Unexpected 'Update' result");

            var updatedMember = _chessClubContext?.Members.First(m => m.Id == id);

            Assert.AreEqual(updateMember.Name, updatedMember?.Name);
            Assert.AreEqual(updateMember.Surname, updatedMember?.Surname);
            Assert.AreEqual(updateMember.Email, updatedMember?.Email);
            Assert.AreEqual(birthday, updatedMember?.Birthday);
        }

        [Test]
        public void TestUpdateMemberInvalidBirthday()
        {
            var updateMember = _chessClubContext?.Members.First();

            Assert.IsNotNull(updateMember);

            var id = updateMember.Id;
            var birthday = DateTime.Now.AddYears(-1).AddDays(1);

            ArgumentOutOfRangeException? ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => { var result = _chessClubService?.UpdateMember(id, birthday: birthday); });

            Assert.NotNull(ex);
            Assert.That(ex?.Message, Is.EqualTo("Members should be at least 1 year old. (Parameter 'birthday')"));
        }
    }
}
