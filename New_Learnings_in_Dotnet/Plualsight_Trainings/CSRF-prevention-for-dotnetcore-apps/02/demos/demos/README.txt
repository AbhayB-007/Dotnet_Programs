Because cookies are set per domain, running both the coffeeshop site and the attacker site on localhost won't work.

When you start the projects you should point the browser to coffeeshop.com and winbitcoin.com respectively and then change the internal DNS of your operating system to point these two domains to localhost.

For Windows:
Start Notepad as administrator and edit the file c:\Windows\System32\Drivers\etc\hosts
Add the following entries and save:

127.0.0.1		coffeeshop.com
127.0.0.1		winbitcoin.com

After that right click on both of the projects in Visual Studio. Go to the debug tab. Under launch make sure "Project" is selected and in the launch browser textbox enter https://coffeeshop.com:[port] and https://winbitcoin.com[port] respectively where [port] is the number the app is running on. You can find the port on the bottom of the debug tab where it says for example: https://localhost:5001. (5001 is the port here)

When you start the projects for the first time the browser will complain that there is no valid TLS certificate for the domains. Ignore the error and continue.