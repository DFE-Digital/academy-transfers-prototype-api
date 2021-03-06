using System.Collections.Generic;
using Data;
using Data.Models;
using Data.Models.KeyStagePerformance;
using Data.Models.Projects;
using Frontend.Controllers;
using Frontend.Models;
using Frontend.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Frontend.Tests.ControllerTests
{
    public class ProjectControllerTests
    {
        private readonly ProjectController _subject;
        private readonly Mock<IProjects> _projectsRepository;
        private readonly Mock<IEducationPerformance> _educationPerformanceRepository;

        public ProjectControllerTests()
        {
            _projectsRepository = new Mock<IProjects>();
            _educationPerformanceRepository = new Mock<IEducationPerformance>();
            _subject = new ProjectController(_projectsRepository.Object, _educationPerformanceRepository.Object);
        }

        public class IndexTests : ProjectControllerTests
        {
            private const string ProjectId = "1234";
            private const string AcademyUrn = "academyUrn";
            public IndexTests()
            {
                _educationPerformanceRepository.Setup(r => r.GetByAcademyUrn(AcademyUrn))
                    .ReturnsAsync(new RepositoryResult<EducationPerformance>
                    {
                        Result = new EducationPerformance
                        {
                            KeyStage2Performance = new List<KeyStage2>()
                        }
                    });
                
                _projectsRepository.Setup(r => r.GetByUrn(ProjectId)).ReturnsAsync(
                    new RepositoryResult<Project>
                    {
                        Result = new Project
                        {
                            Name = "Some name",
                            OutgoingTrustName = "Meow Meowington's Trust",
                            TransferringAcademies = new List<TransferringAcademies>
                            {
                                new TransferringAcademies
                                {
                                    OutgoingAcademyUrn = AcademyUrn
                                }
                            } 
                        }
                    });
            }
            
            [Fact]
            public async void GivenAProjectID_PutsTheProjectNameAndTrustNameInTheViewData()
            {
                var actionResult = await _subject.Index(ProjectId);
                var viewModel = ControllerTestHelpers.GetViewModelFromResult<ProjectTaskListViewModel>(actionResult);

                Assert.Equal("Some name", viewModel.Project.Name);
                Assert.Equal("Meow Meowington's Trust", viewModel.Project.OutgoingTrustName);
            }
            
            [Fact]
            public async void GivenAProjectID_PutsTheHasKeyStage2PerformanceDataInTheViewData()
            {
                await _subject.Index(ProjectId);

                Assert.False((bool)_subject.ViewData["HasKeyStage2PerformanceData"]);
            }
            
            [Fact]
            public async void GivenAProjectWithKeyStage2Data_SetsHasKeyStage2PerformanceDataToTrueInTheViewData()
            {
                _educationPerformanceRepository.Setup(r => r.GetByAcademyUrn(AcademyUrn))
                    .ReturnsAsync(new RepositoryResult<EducationPerformance>
                    {
                        Result = new EducationPerformance
                        {
                            KeyStage2Performance = new List<KeyStage2>
                            {
                                new KeyStage2
                                {
                                    MathsProgressScore = new DisadvantagedPupilsResult() { NotDisadvantaged = "score" }
                                }
                            }
                        }
                    });
                
                await _subject.Index(ProjectId);

                Assert.True((bool)_subject.ViewData["HasKeyStage2PerformanceData"]);
            }

            [Fact]
            public async void GivenGetByUrnReturnsError_DisplayErrorPage()
            {
                const string projectId = "errorProjectId";

                _projectsRepository.Setup(r => r.GetByUrn(projectId)).ReturnsAsync(
                    new RepositoryResult<Project>
                    {
                        Error = new RepositoryResultBase.RepositoryError
                        {
                            ErrorMessage = "Error"
                        }
                    });

                var response = await _subject.Index(projectId);
                var viewResult = Assert.IsType<ViewResult>(response);

                Assert.Equal("ErrorPage", viewResult.ViewName);
                Assert.Equal("Error", viewResult.Model);
            }
        }
    }
}