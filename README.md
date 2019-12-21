# TestTaskForAvito
Тестовое задание для компании Авито. (сервис для постановки встреч)

Приложение (WebAPI) для постановки встреч.

*методы:
- добавление участника (просто добавление в базу) POST https://localhost:XXXXX/Participants + Headers: Content-Type application/json + Body { "name":"name", "email":"email@email.com"}
- вывод всех участников из базы GET https://localhost:XXXXX/Participants

- вывод всех встреч с участниками GET https://localhost:XXXXX/Meetings
- добавление встречи GET https://localhost:XXXXX/Meetings + Headers: Content-Type application/json + 
                                    Body { "name":"name", "datetime": datetime, "meetingParticipantsId" : [id1, id2,id3]}
- удаление встречи DELETE https://localhost:XXXXX/Meetings/meetingId
- добавление участников на встречу GET https://localhost:XXXXX/Meetings/meetingId/Participants + Headers: Content-Type application/json + 
                                    Body {[id1, id2,id3]}
- удаление участника с встречи DELETE https://localhost:XXXXX/Meetings/meetingId/Participant/participantId

*Не реализованно:-За 15 минут до встречи отправить напоминание
                 -Проверка занято/свободно время участников (выдавать предупреждение)

*Для работы с базой данных необходимо поменять строчку "ConnectionStrings": "DefaultConnection" в файле appsettings.json
*Для отправки сообщений необходимо настроить MessageService
