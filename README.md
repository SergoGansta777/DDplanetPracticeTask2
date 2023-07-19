# DDplanetPracticeTask2

Исходный код проекта для решения второго задания практики [ddplanet](https://www.ddplanet.ru).
> Студент ТулГу гр 221111 **Нехорошев С.Р.**

// table of content

## Задание

---
Справочник локаций управляющей компании

Необходимо создать справочник локаций управляющей компании
Сущности:

- Управляющая компания:
  - id
  - название

- Локация (зона действия)
  - id
  - название
  - id управляющей компании (foreign key на таблицу управляющих компаний)

- Адрес в локации:
  - код дома по ФИАС
  - код улицы по ФИАС
  - код города по ФИАС
  - код региона по ФИАС
  - id локации (foreign key на таблицу локаций)

 Адреса в локациях могут быть на любом уровне ФИАС.

 Для адресов в локации заполняются уровни ФИАС выше текущего. Для дома - улица, город, регион. Для улицы - город, регион. Для города - регион.
  
Необходимо реализовать REST API со следующими функциями:

- Добавление, удаление, изменение локаций
- Добавление адресов в локации с условием - у одной укправляющей компании локации не должны пересекаться. Например, невозможна ситуация, когда компания имеет локации:

   1. Тула. В границах одна запись с заполненными городом и регионом
   2. г. Тула, ул. Советская. В границах одна запись с заполненной улицей, городом, регионом.
  Родительские уровни ФИАС (для дома - улица, город, регион. Для улицы - город, регион. Для города - регион) тоже передаются на вход метода добавления адресов. Валидация должна отсекать только незаполненные промежуточные уровни (например если заполнены улица и регион, а город - нет, то это ошибка), никакой интеграции со справочником адресов ФИАС для проверки консистентности (например что улица принадлежит выбранному городу) делать не нужно.
  
- Удаление адреса из локации

---

## База данных

Для выполнения задания была выбрана следущая бд: [_PosgresSql_](https://www.postgresql.org).

### Диaграмма базы данных

// Diagram photo

### Скрипт для создания БД

```sql
-- Создание таблицы управляющих комапаний
CREATE TABLE management_company (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

-- Создание таблицы локаций
CREATE TABLE location (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    management_company_id INTEGER REFERENCES management_company(id) ON DELETE CASCADE
);

-- Создание таблицы адресов в локации
CREATE TABLE address_in_location (
    fias_house_code VARCHAR(4) ,
    fias_street_code VARCHAR(4),
    fias_city_code VARCHAR(3) ,
    fias_region_code VARCHAR(2),
    location_id INTEGER REFERENCES location(id) ON DELETE CASCADE
);
```

### Подключение программы к БД

Внутри файла **appsettings.json** потребуется явно указать _строку подключения_ вида:

```json
"ConnectionStrings": {
    "DefaultConnection": "host=<host_name>;port=<port_number>;database=<database_name>;username=<username>;password=<password>"
  }
```

Сама же строка подключения используется в Program.cs:

```csharp
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddDbContext<PracticeTask1Context>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
```

## Запуск программы

По умолчанию запуск производится на локальном хосте.
Запустить с помощью Visual Studio или ввести в терминале:

```bash
dotnet run
```

В реализации использвались некоторые nuget-пакеты.
Скачать необходимые пакеты можно командами:

```bash
 dotnet add package Microsoft.EntityFrameworkCore
 dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL 
 dotnet add package Microsoft.EntityFrameworkCore.PostgreSQL
 dotnet add package Microsoft.EntityFrameworkCore.Design
```

## Документация

Документация была сгенерирована с помощью [Swagger](https://swagger.io)
// screen

Проверить роботоспособность методов rest-api после запуска можно по следущему URL:
<https://localhost:7033/swagger>
