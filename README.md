# Etiqueta Certa - Desafio

## Solução em video
https://youtu.be/QxCcAbIExgo

## Tecnologias 
- .NET
- SQL SERVER

## Clone o repositório:

```
$ git clone https://github.com/Pedrohbedin/Etiqueta-Certa-Desafio.git
```

## Context
Será necessário fazer a alteração da string de conexação que está localizada no arquivo EtiquetaCertaContext dentro da pasta Context para um dos valores a seguir:
- Autenticação do Windows
  ```
  => optionsBuilder.UseSqlServer("Server = [NomeDoServidor]; DataBase = EtiquetaCerta; TrustServerCertificate = true; Integrated Security=True;");
  ```
- Autenticação do SQL Server
  ```
  => optionsBuilder.UseSqlServer("Server = [NomeDoServidor]; DataBase = EtiquetaCerta; User Id = [Logon]; Pwd = [Senha]; TrustServerCertificate = true");
  ```



## Script do Banco
Foi optado por usar o script baseado no SQL SERVER portanto houve a mudança da sintaxe 

```
CREATE DATABASE EtiquetaCerta;
GO

USE EtiquetaCerta;
GO

CREATE TABLE ConservationProcess(
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(100),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE Symbology(
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    id_process UNIQUEIDENTIFIER,
    name NVARCHAR(100),
    description NVARCHAR(200),
    url NVARCHAR(200),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (id_process) REFERENCES ConservationProcess(id)
);
GO

CREATE TABLE Legislation(
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(100),
    official_language NVARCHAR(100),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE Label(
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    name NVARCHAR(100),
    id_legislation UNIQUEIDENTIFIER,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (id_legislation) REFERENCES Legislation(id)
);
GO

CREATE TABLE LabelSymbology(
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    id_symbology UNIQUEIDENTIFIER,
    id_label UNIQUEIDENTIFIER,
    FOREIGN KEY (id_symbology) REFERENCES Symbology(id),
    FOREIGN KEY (id_label) REFERENCES Label(id)
);
GO

CREATE TABLE SymbologyTranslate(
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    id_symbology UNIQUEIDENTIFIER,
    id_legislation UNIQUEIDENTIFIER,
    symbology_translate NVARCHAR(100),
    FOREIGN KEY (id_symbology) REFERENCES Symbology(id),
    FOREIGN KEY (id_legislation) REFERENCES Legislation(id)
);
GO

CREATE TABLE ProcessInLegislation(
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    id_process UNIQUEIDENTIFIER,
    id_legislation UNIQUEIDENTIFIER,
    FOREIGN KEY (id_process) REFERENCES ConservationProcess(id),
    FOREIGN KEY (id_legislation) REFERENCES Legislation(id)
);
GO

INSERT INTO ConservationProcess (name) VALUES
('Lavagem'),
('Alvejamento'),
('Secagem'),
('Passadoria'),
('Limpeza profissional');
GO

INSERT INTO Symbology (id_process, name, description, url)
VALUES 
((SELECT id FROM ConservationProcess WHERE name = 'Lavagem'), 'lavagem-30-graus', 'Lavagem normal com temperatura máxima de 30 graus celsius', 'https://www.maison-travaux.fr/wp-content/uploads/sites/8/2018/08/un-rincage-normal-et-un-essorage-reduit.gif'),
((SELECT id FROM ConservationProcess WHERE name = 'Lavagem'), 'lavagem-50-graus', 'Lavagem normal com temperatura máxima de 50 graus celsius', 'https://thumbs.dreamstime.com/b/temperatura-da-lavagem-graus-de-s%C3%ADmbolo-%C3%ADcone-do-153330737.jpg'),
((SELECT id FROM ConservationProcess WHERE name = 'Alvejamento'), 'qualquer-alvejante', 'Qualquer alvejante é permitido', 'https://static.thenounproject.com/png/2014515-200.png'),
((SELECT id FROM ConservationProcess WHERE name = 'Alvejamento'), 'alvejantes-clorados', 'Não é permitido alvejantes clorados, apenas com oxigênio', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTD62-QBphtIQUX_tXFciBAoaaHhaBkKHMatpL_k0DdsKgoPOO7MA1RlB3QzLg1pflQLdE&usqp=CAU'),
((SELECT id FROM ConservationProcess WHERE name = 'Secagem'), 'secagem-tambor-temperatura-baixa', 'Possível secagem em tambor com temperatura baixa', 'https://www.uma.com.br/images/icons/cuidados-lavagem/cuidados-28.png'),
((SELECT id FROM ConservationProcess WHERE name = 'Secagem'), 'secagem-tambor-temperatura-normal', 'Possível secagem em tambor com temperatura normal', 'https://oneoffconfeccao.com.br/wp-content/uploads/2018/05/secagem1_oneoffconfeccao.png'),
((SELECT id FROM ConservationProcess WHERE name = 'Passadoria'), 'ferro-110', 'Temperatura máxima da base do ferro de 110 ºC', 'https://1.bp.blogspot.com/_yUJ5ZdEbVDI/TJ4z04QO17I/AAAAAAAAA8k/JRzOvgHtX8U/s1600/bolxnxhku160vnohawvv5i45v.jpg'),
((SELECT id FROM ConservationProcess WHERE name = 'Passadoria'), 'ferro-150', 'Temperatura máxima da base do ferro de 150 ºC', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSePXiiQmeK1mHYZIjs3nI3Hm7JDz-H5XEpBgO0A00zxmEt21gZD2dPmw5BA6CTUpORe3U&usqp=CAU'),
((SELECT id FROM ConservationProcess WHERE name = 'Limpeza profissional'), 'limpeza-seco-tetracloroetileno-normal', 'Limpeza a seco profissional com tetracloroetileno em processo normal', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRVQLtdoWcPaJuW2FPou_0S3yVaPL4sUWWOH2zpUbBLQGSW2Buf26t6wnGll4PaPhztA2Q&usqp=CAU'),
((SELECT id FROM ConservationProcess WHERE name = 'Limpeza profissional'), 'limpeza-seco-tetracloroetileno-suave', 'Limpeza a seco profissional com tetracloroetileno em processo suave', 'https://oneoffconfeccao.com.br/wp-content/uploads/2018/06/simbolos-cuidados-oneoff-confeccao-lavagem-profissional-3.png');
GO
```
