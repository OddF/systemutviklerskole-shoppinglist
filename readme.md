# Kom i gang

## Forutsetninger

Under workshopen kan du velge om du vill sette opp et eget Azure miljø eller låne credentials.
Dersom du ønsker å gjøre dette på egenhånd i Azure må du ha en aktiv subscription, med en Storage Account og en App Service plan.
Demoapplikasjonen er kun testet på windows runtime (relevant når du lager/velger plan).

For å kjøre applikasjonen på lokal maskin trenger du [Node.js](https://nodejs.org/en/download/) og [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) installert 

## Kjøre applikasjon

1. Bytt ut StaticConfiguration.Name i `Program.cs` med ditt eget navn. Ingen mellomrom eller norske tegn
2. Opprett appsettings.Development.json fil, legg inn connection string. Denne får du på kurset eller du kan opprette din egen storage account i Azure og legge til connection string
   * Ligger under *Security + networking* -> *Access keys*
   * filen skal se slik ut:
```
{
   "ConnectionStrings": { "AzureStorage": "DefaultEndpointsProtocol=https;AccountName=NavnPåStorageAccount;AccountKey=<Key>;EndpointSuffix=core.windows.net" }
}
```
3. Hent pakker
   * i denne mappen kjør `dotnet restore` eller restore solution fra Visual studio
   * i `shoppinglist-frontend/` mappen, kjør `npm install`
4. Start backend fra Visual Studio, Visual Studio Code eller `dotnet run`. 
5. Start frontend med `npm run dev`
6. Da skal det være mulig å åpne applikasjonen på [localhost:3000](http://localhost:3000/)

# Oppgaver

Vi skal opprette to GitHub Actions.

## Oppgave 1

Implementere bygg og test av applikasjonen med Github Actions.
Kan gjerne starte med å søke opp template for .NET, Sørg også for at du bygger frontend applikasjonen i byggsteget.

## Oppgave 2

Deploy av applikasjonen til Azure. Her må vi håndtere to secrets.
* AZURE_CREDENTIALS: som vi skal bruke for å logge inn mot azure. Dette trenger vi for å lage webapplikasjonen og gjøre deploy.
* STORAGEACCOUNTCONNECTIONSTRING: Denne trenger applikasjonen vår for å kjøre. Brukes for å hente og lagre items.

Disse legges til som secrets i github. Gå til settings (for repository), *Secrets and variables* -> *Actions* og legg de inn som *Repository secrets*

Ta utgangspunkt i `workflowsToImplement/BuildAndDeployApplication.yaml`, fiks `Set storage account envVar` og kopier over stegene fra oppgave 1.

Dersom du ønsker å generere din egen service principal (AZURE_CREDENTIALS) kan du åpne cloud shell i azure portalen og kjøre denne: `az ad sp create-for-rbac --name "myAppServicePrincipal" --role contributor --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group} --sdk-auth`
