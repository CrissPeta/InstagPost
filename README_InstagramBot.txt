Proyecto: Instagram Auto-Post Robot
Descripción General
Este software es un bot de automatización desarrollado en C# diseñado para la carga masiva y automática de videos (Reels/Posts) a la plataforma Instagram desde un entorno local. El sistema utiliza técnicas de "Browser Automation" para imitar el comportamiento humano, permitiendo gestionar el contenido sin intervención manual constante.

Arquitectura del Sistema
El proyecto se basa en una arquitectura de Automatización de Interfaz de Usuario (UI Automation):

Capa de Aplicación: Consola de .NET que gestiona la lógica de negocio y el flujo de archivos.

Capa de Control: Selenium WebDriver actúa como puente entre el código C# y el navegador.

Capa de Persistencia: Utiliza el sistema de archivos local (System.IO) y los perfiles de usuario de Chrome para mantener sesiones activas (Cookies/Cache).

Tecnologías Utilizadas
Lenguaje: C# (.NET Core).

Librería Principal: Selenium.WebDriver v4.x.

Controlador: ChromeDriver (versión acorde al navegador instalado).

Navegador: Google Chrome.