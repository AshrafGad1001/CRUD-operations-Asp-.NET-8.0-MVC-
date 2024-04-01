
namespace GameZone.Services
{
    public class GamesServices : IGamesServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly string _imagespath;
        public GamesServices(ApplicationDbContext context,
            IWebHostEnvironment WebHostEnvironment)
        {
            _context = context;
            _WebHostEnvironment = WebHostEnvironment;
            _imagespath = $"{_WebHostEnvironment.WebRootPath}{FileSetting.ImagesPath}";
        }
        public IEnumerable<Game> GetAll()
        {
            var games = _context.Games
                                .Include(g => g.Category)
                                .Include(g => g.Devices)
                                .ThenInclude(d => d.Device)
                                .AsNoTracking()
                                .ToList();
            return games;
        }
        public Game? GetById(int id)
        {
            var game = _context.Games
                             .Include(g => g.Category)
                             .Include(g => g.Devices)
                             .ThenInclude(d => d.Device)
                             .AsNoTracking()
                             .SingleOrDefault(g => g.Id == id);

            return game;
        }
        public async Task Create(CreateGameFormViewModel model)
        {
            var coverName = await saveCover(model.Cover);

            Game game = new()
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList(),


            };
            _context.Games.Add(game);
            _context.SaveChanges();
        }

        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = _context.Games
                               .Include(g => g.Devices)
                               .SingleOrDefault(g => g.Id == model.Id);


            if (game is null)
            {
                return null;
            }

            var hasNewCover = model.Cover is not null;
            var oldCover = game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

            if (hasNewCover)
            {
                game.Cover = await saveCover(model.Cover!);
            }


            var effectedRow = _context.SaveChanges();
            if (effectedRow > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagespath, oldCover);
                    File.Delete(cover);
                }
                return game;
            }
            else
            {
                var cover = Path.Combine(_imagespath, game.Cover);
                File.Delete(cover);
                return null;
            }


        }

        public bool Delete(int id)
        {
            var IsDeleted = false;

            var game = _context.Games.Find(id);

            if (game is null)
            {
                return IsDeleted;
            }

            _context.Games.Remove(game);
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                IsDeleted = true;

                var cover = Path.Combine(_imagespath, game.Cover);
                File.Delete(cover);
            }

            return IsDeleted;
        }
        private async Task<string> saveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagespath, coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);
            return coverName;
        }


    }
}
