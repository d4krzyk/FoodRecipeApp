<p align="center">
  <img src="AppGUI/cook-128.ico" alt="App icon" />
</p>

# RecipeApp

## Application Description
RecipeApp is a desktop WPF application for Windows that allows users to search, browse, and save cooking recipes. The application uses an external API (TheMealDB) and a custom Windows Service to manage saved recipes.

## Main Features
- **Recipe Search**: Users can search for recipes by dish name. Results are fetched from an external API.
- **View Recipe Details**: After selecting a recipe, users can view detailed information such as ingredients, instructions, image, category, region, YouTube video link, and source.
- **Save Recipes**: Selected recipes can be saved locally. The list of saved recipes is available in the application.
- **Delete Saved Recipes**: Users can remove recipes from their saved list.
- **Offline Support**: Saved recipes are available even without an internet connection.
- **Windows Service Integration**: Management of saved recipes is handled via a custom Windows Service (AppMealService), which can be started and stopped from the application.

## Project Structure
- **AppGUI**: WPF user interface. Handles navigation, searching, displaying, and user interactions.
- **MealsLibrary1**: Business logic, API communication, file handling, recipe serialization, WCF contract.
- **AppMealService**: Windows Service providing recipe save/read functions via WCF.
- **AppGUITests**: Unit tests for application logic.

## Requirements
- .NET Framework 4.7.2
- Windows 10/11
- Administrator privileges to run the Windows Service

## Installation and Launch
1. **Install the Windows Service**: Build the AppMealService project and install the service using `InstallUtil.exe` or PowerShell.
2. **Run the Application**: Build and run the AppGUI project.
3. **First Launch**: On the welcome screen, click "Start" to launch the service and proceed to recipe search.

## Usage
- Search for a recipe by entering the dish name and clicking "Search".
- Browse results and click a recipe to view details.
- Add a recipe to saved or remove it from the list.
- Browse saved recipes offline.

## Technologies
- WPF (.NET Framework 4.7.2)
- WCF (Windows Communication Foundation)
- Windows Service
- JSON, HTTP

## License
Educational project, for non-commercial use.
