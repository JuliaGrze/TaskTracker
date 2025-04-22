using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;

namespace TaskTrackerTests
{
    public class TasksServiceTest
    {
        //fields
        private readonly ITasksService _tasksService;
        private readonly ITestOutputHelper _outputHelper;
        public TasksServiceTest(ITestOutputHelper testOutputHelper)
        {
            _tasksService = new TasksService();
            _outputHelper = testOutputHelper;
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

        #region GetAllTasks

        //Test 1: The list of tasks should be empty by default (before adding new tasks)
        [Fact]
        public void GetAllTasks_EmptyList()
        {
            //Act
            List<TaskResponse> actual_tasks_response_list = _tasksService.GetAllTasks();

            //Assert
            Assert.NotNull(actual_tasks_response_list);
            Assert.Empty(actual_tasks_response_list);
        }

        //Test 2: The list with few tasks
        [Fact]
        public void GetAllTasks_FewTasks()
        {
            //Arrange
            List<TaskAddRequest> taskAddRequests = new List<TaskAddRequest>()
            {
                new TaskAddRequest() { Title = "Task 1", Description = "Hello" },
                new TaskAddRequest() { Title = "Task 2", Description = "Hello" }
            };

            //Act
            List<TaskResponse> tasks_list_from_add_task = new List<TaskResponse>();
            foreach(TaskAddRequest task in taskAddRequests)
            {
                tasks_list_from_add_task.Add(_tasksService.AddTask(task));
            }
            List<TaskResponse> tasks_response_list_from_getAllTasks = _tasksService.GetAllTasks();

            //Assert
            foreach (TaskResponse expected_task in tasks_list_from_add_task)
            {
                Assert.Contains(expected_task, tasks_response_list_from_getAllTasks);
            }

        }
        #endregion

        #region GetTaskById

        //Test 1: If TaskID is null, it should return null as TaskResponse
        [Fact]
        public void GetTaskByID_TaskIDIsNull()
        {
            //Arrange
            Guid? taskID = Guid.Empty;

            //Act
            TaskResponse taskResponse_from_get = _tasksService.GetTaskById(taskID);

            //Assert
            Assert.Null(taskResponse_from_get);
        }

        //Test 2: If we supply an ivalidID, it should return null as TaskResponse 
        [Fact]
        public void GetTaskByID_InvalidTaskID()
        {
            //Arrange
            Guid? taskID = Guid.NewGuid();

            //Act
            TaskResponse taskResponse_from_get = _tasksService.GetTaskById(taskID);

            //Assert
            Assert.Null(taskResponse_from_get);
        }

        //Test 3: If we supply a validID, it should return the matching task details as TaskRepsonse
        [Fact]
        public void GetTaskByID_validTaskID()
        {
            //Arrange
            TaskAddRequest taskAddRequest = new TaskAddRequest() { Title = "Title", Description = "Description" };
            TaskResponse taskResponse_from_add_request = _tasksService.AddTask(taskAddRequest);

            //Act
            TaskResponse actual_taskResponse_from_get = _tasksService.GetTaskById(taskResponse_from_add_request.TaskID);

            //Assert
            Assert.Equal(taskResponse_from_add_request, actual_taskResponse_from_get);
        }

        #endregion

        #region GetFilteredTasks     

        //Task 1: Empty Search text, should returns AllTasks
        [Fact]
        public void GetFilteredTasks_EmptySearchText()
        {
            // Arrange: Adding tasks
            _outputHelper.WriteLine("Expected:");
            List<TaskResponse> tasks_add_request = ListOfTasksAddedToList();

            //Act
            List<TaskResponse> actual_tasks_response_from_search = _tasksService.GetFilteredTasks(nameof(TaskEntity.Title), "");

            _outputHelper.WriteLine("Actual:");
            foreach (TaskResponse taskResponse in actual_tasks_response_from_search)
            {
                _outputHelper.WriteLine(taskResponse.ToString());
            }

            //Assert
            foreach(TaskResponse taskResponse in tasks_add_request)
            {
                Assert.Contains(taskResponse, actual_tasks_response_from_search);
            }
        }

        //Task 1: Empty Search text, should returns AllTasks
        [Fact]
        public void GetFilteredTasks_SearchByTitle()
        {
            // Arrange: Adding tasks
            _outputHelper.WriteLine("Expected:");
            List<TaskResponse> tasks_add_request = ListOfTasksAddedToList();

            //Act
            List<TaskResponse> actual_tasks_response_from_search = _tasksService.GetFilteredTasks(nameof(TaskEntity.Title), "Task");

            _outputHelper.WriteLine("Actual:");
            foreach (TaskResponse taskResponse in actual_tasks_response_from_search)
            {
                _outputHelper.WriteLine(taskResponse.ToString());
            }

            //Assert
            foreach (TaskResponse taskResponse in tasks_add_request)
            {
                if(taskResponse.Title != null)
                    if(taskResponse.Title.Contains("Task", StringComparison.OrdinalIgnoreCase))
                        Assert.Contains(taskResponse, actual_tasks_response_from_search);
            }
        }

        #endregion

        public List<TaskResponse> ListOfTasksAddedToList()
        {
            TaskAddRequest task1 = new TaskAddRequest() { Title = "Task 1", Description = "Description 1" };
            TaskAddRequest task2 = new TaskAddRequest() { Title = "Task 2", Description = "Description 2" };
            TaskAddRequest task3 = new TaskAddRequest() { Title = "Another Task", Description = "Description 3" };

            List<TaskAddRequest> tasks_add_request = new List<TaskAddRequest>()
            {
                task1, task2, task3
            };

            List<TaskResponse> tasks_response_list_from_add = new List<TaskResponse>();
            foreach (TaskAddRequest taskAddRequest in tasks_add_request)
            {
                tasks_response_list_from_add.Add(_tasksService.AddTask(taskAddRequest));
            }

            List<TaskResponse> tasks_response_from_get = _tasksService.GetAllTasks();
            foreach(TaskResponse taskResponse in tasks_response_from_get)
            {
                _outputHelper.WriteLine(taskResponse.ToString());
            }

            return tasks_response_list_from_add;

        }
    }
}
