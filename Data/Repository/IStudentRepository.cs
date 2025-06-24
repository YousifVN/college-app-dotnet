namespace CollegeApp.Data.Repository;

public interface IStudentRepository : ICollegeRepository<Student>
{
    public Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus);
}