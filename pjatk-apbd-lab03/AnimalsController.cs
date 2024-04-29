using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pjatk_apbd_lab03;

[ApiController]
[Route("api/animals")]
public class AnimalsController : ControllerBase
{
    private readonly string _connectionString;

    public AnimalsController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    // GET: api/animals?orderBy=name
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string orderBy = "name")
        {
            var validSortColumns = new HashSet<string> {"name", "description", "category", "area"};
            orderBy = validSortColumns.Contains(orderBy.ToLower()) ? orderBy : "name";

            var animals = new List<object>();
            var query = $"SELECT IdAnimal, Name, Description, Category, Area FROM Animal ORDER BY {orderBy}";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    animals.Add(new
                    {
                        IdAnimal = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Category = reader.GetString(3),
                        Area = reader.GetString(4)
                    });
                }
            }
            return Ok(animals);
        }

        // POST: api/animals
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Animal model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = "INSERT INTO Animal (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", model.Name);
                command.Parameters.AddWithValue("@Description", model.Description);
                command.Parameters.AddWithValue("@Category", model.Category);
                command.Parameters.AddWithValue("@Area", model.Area);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            return CreatedAtAction(nameof(Get), new { model.IdAnimal }, model);
        }

        // PUT: api/animals/{idAnimal}
        [HttpPut("{idAnimal}")]
        public async Task<IActionResult> Put(int idAnimal, [FromBody] Animal model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = "UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                command.Parameters.AddWithValue("@Name", model.Name);
                command.Parameters.AddWithValue("@Description", model.Description);
                command.Parameters.AddWithValue("@Category", model.Category);
                command.Parameters.AddWithValue("@Area", model.Area);
                await connection.OpenAsync();
                int result = await command.ExecuteNonQueryAsync();
                if (result == 0)
                    return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/animals/{idAnimal}
        [HttpDelete("{idAnimal}")]
        public async Task<IActionResult> Delete(int idAnimal)
        {
            var query = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                await connection.OpenAsync();
                int result = await command.ExecuteNonQueryAsync();
                if (result == 0)
                    return NotFound();
            }
            return NoContent();
        }
    }