# SenderApp_ExampleMVC v1.0
Данное кроссплатформенное одностраничное приложение предназначено для отправки сообщений зарегистрированным в бд пользователям на почту по имеющимся шаблонам.
Шаблоны по умолчанию:
  - Новый год;
  - День рождения;
  - Свой текст (custom message).

Проект имеет возможность управления пользователями (добавление, удаление, смена пароля, смена email) и назначениями ролей - admin, user, manager и т.д.
Проект реализовать с помощью паттерна MVC.

Для первичного доступа в базе данных MySql создается (автоматически) пользователь с правами администратора и полным доступом:
логин: sa001@mail.ru
пароль: 12345

"FirstAdminLogin"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/LoginUser.png "FirstAdminLogin")

Админ или другой установленный пользователь которому дали на это право, может просматривать список зарегистрированных в системе пользователей, список ролей, отправлять сообщения по шаблонам и т.д.

"Messages"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/Messages.png "Messages")

"Roles list"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/RolesList.png "Roles list")

"Change user role"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/ChangeUserRole.png "Change user role")

"Admin can write message by special time or now"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/AdminCanWriteMessageByTimeOrNow.png "Admin can write message by special time or now")

Регистрация пользователя с действующим email, т.к. для регистрации необходимо подтверждение email в почте.

"User registration"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/RegisterUser.png "User registration")

После добавления логина и пароля пользователь получает сообщение с просьбой проверить свою почту для подтверждения email.

"After registration"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/AfterRegistration.png "After registration")

Пользователь получает такое письмо (выполненное по шаблону).

"Email confirmation message"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/ConfirmingMessage.png "Email confirmation message")

После подтверждения письма пользователь авторизовывается вводя логин и пароль, далее видим меню.

"After user confirm message"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/AfterUserConfirmMessage.png "After user confirm message")

Следует учесть, что для примера, в качестве сервиса отправки сообщений используется MailKit и для "боевой работы" не подойдет, нужно использовать другие сервисы.
В коде отправки сообщений нужно поменять "your_real_email@mail.ru" и "your_email_password" на действующий email и действующий пароль от почты. 
Кроме того, нужно посмотреть настройки smtp того сервиса с которого будут отправляться сообщения (gmail, mail, yahoo и т.д.). 

В проект также добавлены:
 - Логирование Serilog запись в консоль и в файл, директория LOGS;
 - Mapper;
 - MySql;
 - MailKit;
 - Microsoft.AspNetCore.Identity.EntityFrameworkCore;
 - Quartz (отправка сообщений по времени);
 - RazorEngine.NetCore (шаблонизатор);
 - Docker-compose file.
