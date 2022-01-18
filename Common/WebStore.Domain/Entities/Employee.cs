using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities;

public class Employee : Entity
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Patronymic { get; set; }

    public int Age { get; set; }
}
