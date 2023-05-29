# kpo4
Проект реализован на языке C#, используя фреймворк API.net. База данных использовалась PostgreSQL с названием kpo2. Обработаны различные некорректные ситуации путём возврата соответсвующего ошибке кода HTTP, с описанием ошибки.

SQL для таблиц:
CREATE TABLE kpo2.public.session (
    id SERIAL PRIMARY KEY,
    user_id INT NOT NULL,
    session_token VARCHAR(255) NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    FOREIGN KEY (user_id) REFERENCES kpo2.public.user (id)
);

CREATE TABLE kpo2.public.user (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    role VARCHAR(10) NOT NULL CHECK (role IN ('customer', 'chef', 'manager')),
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);
