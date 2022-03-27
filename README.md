# dashboard-web
A dashboard for displaying data from solarpower and weather forecast

The dashboard is password protected.

Therefore you have use one of these options to run the app.

#1 Take out the [Authorize] attribute out af the controllers that way you won't have to log in.
After that go th DashboardController and hardcode username and password in the following methods
- CallWebService("Kolding", credential.Password);
- string file = fn.GetFileNameMethod(cd.UserName, cd.Password);
- string a = GetFileAndOutPut(cd.UserName, cd.Password, file);

Build and run the project.
