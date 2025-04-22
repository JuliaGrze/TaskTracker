using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace TaskTrackerTests
{
    public class TasksServiceTest
    {
        //fields
        private readonly ITasksService _tasksService;
        public TasksServiceTest()
        {
            _tasksService = new TasksService();
        }

        #region AddTask

        //Test 1: when TaskAddRequest in null, should retrun ArgumentNullException
        [Fact]
        public void AddTask_NullTaskAddRequest()
        {
            //Arrange
            TaskAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            //act
            _tasksService.AddTask(request));
        }

        //Test 2: When task title is empty, should return ArgumentException
        [Fact]
        public void AddTask_TitleIsNull()
        {
            //Arrange: task title is null
            TaskAddRequest? taskAddRequest = new TaskAddRequest() { Title = null };

            //Assert & act
            Assert.Throws<ArgumentException>(() => _tasksService.AddTask(taskAddRequest));
        }

        //Test 3: When task title is shorter than 3 chars, it should return ArgumentException
        [Fact]
        public void AddTask_TitleIsTooShort()
        {
            //Arrange: task title is too short
            TaskAddRequest? taskAddRequest = new TaskAddRequest() { Title = "Hi"};

            //Assert & act
            Assert.Throws<ArgumentException>(() => _tasksService.AddTask(taskAddRequest));
        }

        //Test 4: Valid data, it should insert task into task list and it should return an object of TaskResponse, which includes with the newly generated task id
        [Fact]
        public void AddTask_ProperData()
        {
            //Arrange: valid data
            TaskAddRequest? taskAddRequest = new TaskAddRequest() { Title = "Running", Description = "Running 10 km" };

            //Act
            TaskResponse taskResponse = _tasksService.AddTask(taskAddRequest);

            //Assert
            Assert.True(taskResponse.TaskID != Guid.Empty);
        }

        #endregion
    }
}
