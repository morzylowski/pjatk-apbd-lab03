using System.ComponentModel.DataAnnotations;

namespace pjatk_apbd_lab03;

public class Animal
{
    public int IdAnimal { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; }

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [StringLength(200, ErrorMessage = "Category cannot exceed 200 characters")]
    public string Category { get; set; }

    [Required(ErrorMessage = "Area is required")]
    [StringLength(200, ErrorMessage = "Area cannot exceed 200 characters")]
    public string Area { get; set; }
}
