namespace CollegeApp.Data.Repository;

public class StudentRepository : CollegeRepository<Student>, IStudentRepository
{
    private readonly CollegeDbContext _dbContext;

    public StudentRepository(CollegeDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus)
    {
        // write logic here
        return null;
    }

    
}