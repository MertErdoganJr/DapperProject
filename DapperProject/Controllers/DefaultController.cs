using Dapper;
using DapperProject.DapperContext;
using DapperProject.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DapperProject.Controllers
{
    public class DefaultController : Controller
    {
        private readonly Context _context;

        public DefaultController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string query = "Select * From TblProject";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultProjectDto>(query);
            return View(values.ToList());
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
        {
            string query = "insert into TblProject(ProjectTitle,ProjectDescription,ProjectImageUrl) values (@title,@description,@imageUrl)";
            var paramaters = new DynamicParameters();
            paramaters.Add("@title", createProjectDto.ProjectTitle);
            paramaters.Add("@description", createProjectDto.ProjectDescription);
            paramaters.Add("@imageUrl", createProjectDto.ProjectImageUrl);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, paramaters);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProject(int id)
        {
            string query = "Delete From TblProject where ProjectID=@ProjectID";
            var parameters = new DynamicParameters();
            parameters.Add("@ProjectID", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProject(int id)
        {
            string query = "Select * From TblProject where ProjectID=@ProjectID";
            var parameters = new DynamicParameters();
            parameters.Add("@ProjectID", id);
            var connection = _context.CreateConnection();
            var values = await connection.QueryFirstOrDefaultAsync<UpdateProjectDto>(query);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProject(UpdateProjectDto updateProjectDto)
        {
            string query = "Update TblProject Set ProjectTitle=@title,ProjectDescription=@description,@ProjectImageUrl=@imageurl where ProjectID=@ProjectID";
            var paramaters = new DynamicParameters();
            paramaters.Add("@ProjectID", updateProjectDto.ProjectID);
            paramaters.Add("@title", updateProjectDto.ProjectTitle);
            paramaters.Add("@description", updateProjectDto.ProjectDescription);
            paramaters.Add("@imageUrl", updateProjectDto.ProjectImageUrl);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, paramaters);
            return RedirectToAction("Index");
        }
    }
}
