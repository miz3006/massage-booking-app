**Spletna aplikacija za rezervacijo azijskih masaž**

https://rezervacijamasazweb-gmcqfmgeewh8fnaw.swedencentral-01.azurewebsites.net/Home 

Člana ekipe:

**63240212 Edis Mizić**

**63240010 Aleks Ašanin**

V današnjem hitrem svetu, kjer nam časa vedno primanjkuje in je tempo življenja vse bolj dinamičen, postaja udobje in enostaven dostop do storitev ključnega pomena. Ljudje si želimo preprostih rešitev, ki nam prihranijo čas in nam omogočajo, da stvari uredimo hitro in brez zapletov.

Z mislijo na to sva se odločila razviti spletni rezervacijski sistem za azijske masaže – sodobno in uporabniku prijazno aplikacijo, ki poenostavi poslovanje masažnega salona in izboljša izkušnjo strank. Sistem bo omogočal enostavno spletno rezervacijo termina brez telefonskega klica ali osebnega obiska. Uporabnik bo lahko izbral vrsto masaže, trajanje, izvajalca ter termin, ki mu najbolj ustreza – vse to udobno in hitro, kar prek računalnika ali telefona.

Za ogled ponudbe in opis masaž registracija ne bo potrebna. Če pa bo uporabnik želel opraviti rezervacijo, se bo lahko hitro registriral in pridobil svoj profil. Po uspešni rezervaciji bo sistem samodejno označil termin kot zaseden v salonovem koledarju.

Prednost sistema bo delovanje v realnem času – vsi podatki o rezervacijah in prostih terminih se bodo takoj posodabljali, ne glede na to, ali jih vnese stranka ali osebje. Tako se bodo preprečile dvojne rezervacije in zagotovila popolna usklajenost med spletnim in fizičnim poslovanjem.

Na strani zaposlenih bo sistem omogočal pregled in upravljanje terminov, osebja ter razpoložljivosti. Osebje bo lahko dodajalo nove zaposlene, spreminjalo razpoložljivost in spremljalo povratne informacije uporabnikov.

S tem projektom želiva ustvariti sistem, ki združuje udobje, preglednost in zanesljivost v eni rešitvi. Strankam bo omogočal hitro pot do sprostitve, salonu pa učinkovito orodje za organizacijo dela. Verjameva, da bo takšen sistem sodoben korak naprej pri digitalizaciji poslovanja masažnih salonov in bo predstavljal konkurenčno prednost na trgu.

<img width="1654" height="945" alt="image" src="https://github.com/user-attachments/assets/216db94b-0e50-47a3-9f06-3129755a1b2a" />
Slika 1: Začetna stran spletne aplikacije
<img width="1648" height="902" alt="image" src="https://github.com/user-attachments/assets/d3975c0c-c1df-43fb-b59a-2d34f5e49a94" />
Slika 2: Izpolnjna forma za rezervacijo mazaže
<img width="1648" height="912" alt="image" src="https://github.com/user-attachments/assets/34f22e03-b9b6-41ea-9bce-59ab87794d75" />
Slika 3: Podstran masaže v admin dashboardu, kjer lahko admin ureja in dodaja nove masaže

Slika 4 in 5: Mobilna aplikacija (izpis in dodajanje novih masaž)

<img width="631" height="654" alt="image" src="https://github.com/user-attachments/assets/6f0798f3-bfa4-4886-905b-9a1891f829c1" />
Slik 6: Shema podatkovne baze
Podatkovni model podpira upravljanje uporabnikov, masaž, maserjev, terminov, rezervacij in obvestil. Uporabniki (AspNetUsers) se uporabljajo za avtentikacijo ter so povezani z rezervacijami in obvestili. Tabele Masaze in Maser predstavljajo storitev in izvajalcev, ki se povezujejo z rezervacijami. Tabela Termin določa razpoložljivost maserjev po datumih in urah, medtem ko rezervacija združuje podatke o stranki, izbranem maserju, masaži in terminu. Sistem omogoča tudi obveščanje uporabnikov preko tabele obvestilo.






