public abstract class BaseRepository
{
  protected readonly Projeto360Context _context;

  protected BaseRepository(Projeto360Context context)
  {
    _context = context;
  }
}