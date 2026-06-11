using System;
using System.IO;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

// --- CONFIGURACIÓN ---
string miRutaVideos = @"C:\Users\Usuario\Documents\Projects\InstagPost\media";
// Cambia esto por tu ruta real de Chrome (la que sacaste en chrome://version/)
string miPerfilChrome = @"C:\Users\Usuario\AppData\Local\Google\Chrome\User Data\Profile 3"; 

Console.WriteLine("🚀 Iniciando Robot de Instagram...");

// 1. Configurar Chrome
var options = new ChromeOptions();
options.AddArgument($"--user-data-dir={miPerfilChrome}");
options.AddArgument("--profile-directory=Profile 3"); // O "Profile 1", "Profile 2", etc.
//options.EnableMobileEmulation("Pixel 7");// Engañamos a IG para que deje subir fotos
options.AddExcludedArgument("enable-automation");

using var driver = new ChromeDriver(options);
var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

try 
{
 
    // Buscar videos en la carpeta
    var videos = Directory.GetFiles(miRutaVideos, "*.MP4");
    
    if (videos.Length == 0) {
        Console.WriteLine("❌ No encontré videos .mp4 en " + miRutaVideos);
        return;
    }

    foreach (var video in videos)
    {
        driver.Navigate().GoToUrl("https://www.instagram.com/");
         await Task.Delay(2000);
        Console.WriteLine($"📸 Intentando subir: {Path.GetFileName(video)}");

        // Clic en botón "Inicio" (+)
        // var btnInicio = wait.Until(driver =>
        // driver.FindElement(By.XPath("//*[text()='Inicio']/ancestor::*[@role='button' or @role='img'][1]")));
        // btnInicio.Click();
        // var btnInicio = wait.Until(d => d.FindElement(By.XPath("//*[@aria-label='Inicio']")));
        // btnInicio.Click();
        
        // Clic en botón "Nueva publicación" (+)
        var btnCrear = wait.Until(d => d.FindElement(By.XPath("//*[@aria-label='Nueva publicación']")));
        btnCrear.Click();
        await Task.Delay(2000);

        var btnPublicacion = wait.Until(driver =>
        driver.FindElement(By.XPath("//*[text()='Publicación']/ancestor::*[@role='button' or @role='link'][1]")));
        btnPublicacion.Click();

   
        // Subir el archivo (buscamos el input invisible)
        var inputArchivo = wait.Until(d => d.FindElement(By.XPath("//input[@type='file']")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = '';", inputArchivo); // <--- LÍNEA CLAVE
        inputArchivo.SendKeys(video);
        await Task.Delay(4000);
        
        //Seleccionamos el tipo de Ajuste del video al formato cuadrado (1:1)
        var btnRecorte = wait.Until(driver =>
        driver.FindElement(By.XPath("//*[text()='Seleccionar recorte']/ancestor::*[@role='button' or @role='link'][1]")));
        btnRecorte.Click();

        var btnOriginal = wait.Until(driver =>
        driver.FindElement(By.XPath("//*[text()='Original']/ancestor::*[@role='button' or @role='link'][1]")));
        btnOriginal.Click();

        // Clic en "Siguiente" (Ajuste)
        ClickBoton(driver, "Siguiente", "Next");
        await Task.Delay(2000);

        // Clic en "Siguiente" (Filtros)
        ClickBoton(driver, "Siguiente", "Next");
        await Task.Delay(2000);

        // Escribir el texto (Caption)
        var txtCaption = driver.FindElement(By.XPath("//*[@aria-label='Escribe un pie de foto o vídeo…']"));
        txtCaption.SendKeys("#Sevilla #Andalucía #España #DescubreSevilla #Viajar #Turismo #Aventura #Cultura #Historia #Gastronomía #Arte #Arquitectura #Paisajes #ExperienciasÚnicas");
        // await Task.Delay(2000);
        // var txtCaption = driver.FindElement(By.XPath("//*[@aria-label='Escribe un pie de foto o vídeo…']"));
        // txtCaption.SendKeys("#Viral #ExperienciasÚnicas");
        await Task.Delay(2000);
     

        // Clic en "Compartir"
        ClickBoton(driver, "Compartir", "Share");
        
        Console.WriteLine("✅->-> Publicando......");
        await Task.Delay(30000); // Esperar un poco antes de la siguiente
       
        Console.WriteLine("⏳ Esperando confirmación de publicación...");

        bool publicado = false;

        while (!publicado)
        {
           try
            {
                // Buscar el mensaje de confirmación
                var mensaje = driver.FindElement(
                    By.XPath("//*[contains(text(), 'Se ha compartido tu reel')]")
                );

                if (mensaje != null)
                {
                    Console.WriteLine("🎉 Reel compartido detectado!");
                    publicado = true;

                    // Buscar y presionar el botón Cerrar
                    var btnCerrar = wait.Until(driver =>
                    driver.FindElement(By.XPath("//*[text()='Cerrar']/ancestor::*[@role='button' or @role='link'][1]")));
                    btnCerrar.Click();
                }
            }
            catch
            {
                // Si no aparece aún, seguimos esperando
                Thread.Sleep(1000);
            }
        }

    }
}
catch (Exception ex) 
{
    Console.WriteLine("🛑 Error: " + ex.Message);
}
finally 
{
    driver.Quit();
}

// Función auxiliar para hacer clic en botones por texto
void ClickBoton(IWebDriver d, string textoEs, string textoEn) {
    var btn = d.FindElement(By.XPath($"//div[@role='button'][contains(text(),'{textoEs}') or contains(text(),'{textoEn}')]"));
    btn.Click();
}