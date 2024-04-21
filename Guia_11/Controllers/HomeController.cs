using Guia_11.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Firebase.Auth;
using Firebase.Storage;
namespace Guia_11.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        {



            //Leemos el archivo subido. Stream archivoASubir archivo. OpenReadStream();
            Stream archivoAsubir = archivo.OpenReadStream();

            //Configuramos la conexion hacia FireBase
            string email = "jose.erazo3@catolica.edu.sv"; // Correo para autenticar en FireBase
            string clave = "Hola.1234"; // Contraseña establaecida en la autenticar en Firebase
            string ruta = "onion2402.appspot.com"; // URL donde se guardaran los archivo. string api_key = "AIzaSyAhSFON@lsHDGE "
            string api_key = "AIzaSyCVYiVuHXRroEzcOJ1K7jdTZQprAC7afBo";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;


            var tareaCargarArchivo = new FirebaseStorage(ruta,
                                                        new FirebaseStorageOptions
                                                        {
                                                            AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
                                                            ThrowOnCancel = true
                                                        }

                                                    ).Child("Arhivos")
                                                    .Child(archivo.FileName)
                                                    .PutAsync(archivoAsubir, cancellation.Token);

            var urlArchivoCargado = await tareaCargarArchivo;


            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
