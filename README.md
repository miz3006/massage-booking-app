 Spletna aplikacija za rezervacijo azijskih masaž

**Aplikacija:** https://massage-booking-app2-a2ayhkf8cyc8hed5.swedencentral-01.azurewebsites.net

---

## Člana ekipe

| Vpisna št. | Ime in priimek |
|---|---|
| 63240212 | Edis Mizić |
| 63240010 | Aleks Ašanin |

---

## Opis projekta

Spletna aplikacija za **rezervacijo azijskih masaž**, namenjena masažnim salonom in njihovim strankam. Stranka brez telefonskega klica ali osebnega obiska v nekaj klikih rezervira termin — izbere vrsto masaže, trajanje, maserja in željeni čas.

Aplikacija je dostopna na računalniku in mobilnih napravah.

---

## Funkcionalnosti

**Stranka:**
- Ogled masaž in opisa storitev brez registracije
- Registracija in upravljanje lastnega profila
- Spletna rezervacija termina
- Pregled lastnih rezervacij

**Administrator:**
- Upravljanje masaž, maserjev in terminov
- Pregled vseh rezervacij
- Pošiljanje obvestil strankam
- Statistika rezervacij

---

## Mobilna aplikacija

Android admin aplikacija omogoča:
- Pregled in dodajanje masaž
- Statistiko rezervacij (skupaj, danes, mesec)
- Lestvico po masaži in maserju

Komunikacija poteka prek REST API-ja z avtentikacijo prek API ključa.

---

## Razdelitev dela

- **Edis Mizić** — backend, poslovna logika, rezervacijski sistem, API, Android app
- **Aleks Ašanin** — frontend, vizualna podoba, uporabniška izkušnja

Podatkovno bazo in Android vmesnik sva razvijala skupaj.

---

## Prikaz aplikacije

**Slika 1:** Začetna stran spletne aplikacije  

<img width="1654" height="945" alt="Začetna stran" src="https://github.com/user-attachments/assets/216db94b-0e50-47a3-9f06-3129755a1b2a" />

---

**Slika 2:** Izpolnjena forma za rezervacijo masaže  

<img width="1648" height="902" alt="Rezervacija" src="https://github.com/user-attachments/assets/d3975c0c-c1df-43fb-b59a-2d34f5e49a94" />

---

**Slika 3:** Admin podstran za urejanje in dodajanje masaž  

<img width="1638" height="918" alt="image" src="https://github.com/user-attachments/assets/182bdb2a-130e-49e6-a267-c13e5b3c230b" />


---

**Slika 4:** Shema podatkovne baze  

<img width="631" height="654" alt="Shema baze" src="https://github.com/user-attachments/assets/6f0798f3-bfa4-4886-905b-9a1891f829c1" />

---

**Slika 5 in 6:** Mobilna apliakcija 

<img width="349" height="614" alt="image" src="https://github.com/user-attachments/assets/cd3681eb-0160-42c7-8e36-ec258232d816" />
<img width="340" height="611" alt="image" src="https://github.com/user-attachments/assets/5411562c-eec5-4122-af8e-e48b2ef795c4" />

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
