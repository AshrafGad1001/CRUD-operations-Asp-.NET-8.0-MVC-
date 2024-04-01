namespace GameZone.Controllers
{
    public class GamesController : Controller
    {

        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesServices _DevicesService;
        private readonly IGamesServices _gamesService;
        public GamesController(ICategoriesService categoriesService,
               IDevicesServices devicesService, IGamesServices gamesServices)
        {
            _categoriesService = categoriesService;
            _DevicesService = devicesService;
            _gamesService = gamesServices;
        }
        public IActionResult Index()
        {
            var games = _gamesService.GetAll();

            return View(games);
        }
        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                Categoies = _categoriesService.GetSelectList(),
                Devices = _DevicesService.GetSelectList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categoies = _categoriesService.GetSelectList();
                model.Devices = _DevicesService.GetSelectList();

                return View(model);
            }

            //Save Game in DataBase
            await _gamesService.Create(model);
            //Save Cover In Server
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = _gamesService.GetById(id);

            if (game is null)
            {
                return NotFound();
            }


            EditGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categoies = _categoriesService.GetSelectList(),
                Devices = _DevicesService.GetSelectList(),
                CurrentCover = game.Cover
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categoies = _categoriesService.GetSelectList();
                model.Devices = _DevicesService.GetSelectList();

                return View(model);
            }

            var game = await _gamesService.Update(model);

            if (game is null)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));

        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gamesService.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }

}

