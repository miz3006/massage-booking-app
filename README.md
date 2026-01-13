# **Spletna aplikacija za rezervacijo azijskih masaž**

**Povezava do aplikacije:**  
https://rezervacijamasazweb-gmcqfmgeewh8fnaw.swedencentral-01.azurewebsites.net/Home  

---

## Člana ekipe

- **63240212 – Edis Mizić**  
- **63240010 – Aleks Ašanin**

---

## Opis projekta

V današnjem hitrem svetu, kjer nam pogosto primanjkuje časa in je tempo življenja vse bolj dinamičen, postajata udobje in enostaven dostop do storitev ključnega pomena. Uporabniki si želimo preprostih in učinkovitih rešitev, ki nam omogočajo hitro urejanje opravkov brez nepotrebnih zapletov.

Z mislijo na to sva razvila **spletno aplikacijo za rezervacijo azijskih masaž**, ki masažnim salonom omogoča lažje poslovanje, strankam pa prijetno in enostavno uporabniško izkušnjo. Aplikacija omogoča **hitro spletno rezervacijo termina** brez telefonskega klica ali osebnega obiska.

Uporabnik lahko izbere:
- vrsto masaže,
- trajanje,
- maserja,
- termin, ki mu najbolj ustreza.

Vse to je dostopno prek računalnika ali mobilne naprave.

---

## Uporabniška izkušnja

- **Registracija ni potrebna** za ogled ponudbe in opisov masaž.  
- Za izvedbo rezervacije se uporabnik hitro registrira in ustvari svoj profil.  
- Po uspešni rezervaciji sistem termin **samodejno označi kot zaseden** v koledarju salona.  

Posebna prednost sistema je **delovanje v realnem času**, saj se vsi podatki o prostih in zasedenih terminih takoj posodobijo. To preprečuje dvojne rezervacije in zagotavlja usklajenost med strankami in osebjem.

---

## Administrativni del

Za zaposlene in administratorje aplikacija omogoča:
- pregled in upravljanje terminov,
- upravljanje osebja in njihove razpoložljivosti,
- dodajanje in urejanje masaž,
- vpogled v rezervacije in povratne informacije uporabnikov.

---

## Razdelitev dela in sodelovanje

Projekt sva si razdelila tako, da se je:
- **eden izmed naju** osredotočil predvsem na **backend in poslovno logiko**, vključno z rezervacijami, administrativnim delom, upravljanjem terminov in drugimi funkcionalnimi podrobnostmi,
- **drugi** pa je prevzel **frontend**, kjer je skrbel za vizualno podobo in uporabniško izkušnjo.

**Podatkovno bazo sva razvijala skupaj**, pri čemer sva se ves čas dopolnjevala. Pogosto je eden opazil pomanjkljivosti ali možnosti za izboljšave, ki jih je drugi sprva spregledal, nato pa sva jih skupaj predebatirala in nadgradila.

Na enak način sva sodelovala tudi pri razvoju **Android vmesnika**, kjer sva si izmenjevala ideje in iskala najbolj optimalne rešitve.

---

## Namen in cilj projekta

S projektom želiva ustvariti sistem, ki združuje:
- **udobje za uporabnike**,  
- **preglednost podatkov**,  
- **zanesljivost delovanja**.

Strankam omogoča hitro in enostavno pot do sprostitve, masažnemu salonu pa učinkovito orodje za organizacijo dela. Takšen sistem predstavlja pomemben korak k digitalizaciji poslovanja in konkurenčno prednost na trgu.

---

## Prikaz aplikacije

**Slika 1:** Začetna stran spletne aplikacije  

<img width="1654" height="945" alt="Začetna stran" src="https://github.com/user-attachments/assets/216db94b-0e50-47a3-9f06-3129755a1b2a" />

---

**Slika 2:** Izpolnjena forma za rezervacijo masaže  

<img width="1648" height="902" alt="Rezervacija" src="https://github.com/user-attachments/assets/d3975c0c-c1df-43fb-b59a-2d34f5e49a94" />

---

**Slika 3:** Admin podstran za urejanje in dodajanje masaž  

<img width="1648" height="912" alt="Admin masaže" src="https://github.com/user-attachments/assets/34f22e03-b9b6-41ea-9bce-59ab87794d75" />

---

**Sliki 4 in 5:** Mobilna aplikacija – izpis in dodajanje novih masaž  

<img width="354" height="462" alt="Mobilna aplikacija 1" src="https://github.com/user-attachments/assets/7809025d-25ce-489f-a69e-7738519e84c6" />
<img width="345" height="456" alt="Mobilna aplikacija 2" src="https://github.com/user-attachments/assets/20a32b5c-4f79-4e55-af66-534dd0efffe3" />

---

**Slika 6:** Shema podatkovne baze  

<img width="631" height="654" alt="Shema baze" src="https://github.com/user-attachments/assets/6f0798f3-bfa4-4886-905b-9a1891f829c1" />

---

## Opis podatkovnega modela

Podatkovni model omogoča upravljanje:
- uporabnikov,
- masaž,
- maserjev,
- terminov,
- rezervacij,
- obvestil.

Uporabniki (**AspNetUsers**) se uporabljajo za avtentikacijo ter so povezani z rezervacijami in obvestili.  
Tabeli **Masaze** in **Maser** predstavljata storitve in izvajalce, ki se povezujejo z rezervacijami.  
Tabela **Termin** določa razpoložljivost maserjev po datumih in urah, medtem ko **Rezervacija** združuje podatke o stranki, izbranem maserju, masaži in terminu.  
Sistem omogoča tudi obveščanje uporabnikov preko tabele **Obvestilo**.

---
