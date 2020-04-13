-- Criação da tabela Mesa
CREATE TABLE MESA (
	NR_MESA INTEGER NOT NULL PRIMARY KEY
);

-- Criação da tabela Conta
CREATE TABLE CONTA (
	NR_CONTA INTEGER NOT NULL PRIMARY KEY AUTO_INCREMENT,
    DATA_CONTA DATE NOT NULL,
    VALOR_RECEBIDO FLOAT,
    NR_MESA INTEGER NOT NULL,
    FOREIGN KEY (NR_MESA) REFERENCES MESA(NR_MESA) ON DELETE RESTRICT
)AUTO_INCREMENT = 1;

-- Criação da tabela Garcon
CREATE TABLE GARCON (
	NR_GARCON INTEGER NOT NULL PRIMARY KEY,
    NOME VARCHAR(50) NOT NULL
);

-- Criação da tabela Prato
CREATE TABLE PRATO (
	ID_PRATO INTEGER NOT NULL PRIMARY KEY,
    NOME VARCHAR(40) NOT NULL,
    PRECO_UNITARIO FLOAT NOT NULL
);

-- Criação da tabela Pedidos
CREATE TABLE PEDIDOS (
	NR_CONTA INTEGER NOT NULL,
    NR_PRATO INTEGER NOT NULL,
	NR_SEQUENC INTEGER NOT NULL,
    PREC_UNIT FLOAT NOT NULL,
    QUANTIDADE INTEGER NOT NULL,
    ID_GARCON INTEGER NOT NULL,
    DATA_PAG_COMISSAO DATE,
    ENTREGUE BOOL NOT NULL,
    PRIMARY KEY (NR_CONTA, NR_PRATO, NR_SEQUENC),
    FOREIGN KEY (NR_CONTA) REFERENCES CONTA(NR_CONTA) ON DELETE RESTRICT,
    FOREIGN KEY (ID_GARCON) REFERENCES GARCON(NR_GARCON) ON DELETE RESTRICT,
    FOREIGN KEY (NR_PRATO) REFERENCES PRATO(ID_PRATO) ON DELETE RESTRICT
);

-- Inserção de 10 mesas
INSERT INTO MESA VALUES(1);
INSERT INTO MESA VALUES(2);
INSERT INTO MESA VALUES(3);
INSERT INTO MESA VALUES(4);
INSERT INTO MESA VALUES(5);
INSERT INTO MESA VALUES(6);
INSERT INTO MESA VALUES(7);
INSERT INTO MESA VALUES(8);
INSERT INTO MESA VALUES(9);
INSERT INTO MESA VALUES(10);

-- Inserção de 5 garçons
INSERT INTO GARCON VALUES(1, 'Isaque');
INSERT INTO GARCON VALUES(2, 'Vanderson');
INSERT INTO GARCON VALUES(3, 'Weslen');
INSERT INTO GARCON VALUES(4, 'Héricles');
INSERT INTO GARCON VALUES(5, 'Fernanda');

-- Inserção de 20 pratos
INSERT INTO PRATO VALUES(1, 'Doce de batata doce', 10.50);
INSERT INTO PRATO VALUES(2, 'Churrasco', 55.90);
INSERT INTO PRATO VALUES(3, 'Bala de banana Oliveira ou similares', 3.10);
INSERT INTO PRATO VALUES(4, 'Tapioca', 4.50);
INSERT INTO PRATO VALUES(5, 'Pizza assado no forno à lenha', 8.99);
INSERT INTO PRATO VALUES(6, 'Feijão tropeiro', 10.00);
INSERT INTO PRATO VALUES(7, 'Arroz carreteiro', 15.10);
INSERT INTO PRATO VALUES(8, 'Açaí na tijela', 14.99);
INSERT INTO PRATO VALUES(9, 'Paçoca de amendoim', 3.60);
INSERT INTO PRATO VALUES(10, 'Pato no tucupi', 36.50);
INSERT INTO PRATO VALUES(11, 'Maniçoba', 14.10);
INSERT INTO PRATO VALUES(12, 'Baião de dois', 10.50);
INSERT INTO PRATO VALUES(13, 'Acarajé', 9.99);
INSERT INTO PRATO VALUES(14, 'Pamonha', 8.90);
INSERT INTO PRATO VALUES(15, 'Dobradinha', 4.10);
INSERT INTO PRATO VALUES(16, 'Rapadura', 2.40);
INSERT INTO PRATO VALUES(17, 'Farofa de içá', 9.50);
INSERT INTO PRATO VALUES(18, 'Barreado', 8.30);
INSERT INTO PRATO VALUES(19, 'Pastel de feira', 4.00);
INSERT INTO PRATO VALUES(20, 'Couve refogada com alho', 2.50);

-- TESTE CHECK_DISPONILIDADE_MESA -- 

-- 1) Checando se existe uma mesa não reservada disponível
-- SET @TEST = -1;

-- CALL CHECK_DISPONIBILIDADE_MESA (1, @TEST);

-- SELECT @TEST;

-- 2) Checando se uma mesa reservada está disponível
-- INSERT INTO CONTA(DATA_CONTA, VALOR_RECEBIDO, NR_MESA) VALUES('2018-03-26', NULL, 1);

-- SELECT * FROM CONTA;

-- CALL CHECK_DISPONIBILIDADE_MESA (1, @TEST);

-- SELECT @TEST;

-- FIM DOS TESTES DO CHECK_DISPONILIDADE_MESA --

DELIMITER //
CREATE PROCEDURE CHECK_DISPONIBILIDADE_MESA (IN NR_MESA INTEGER, OUT VALOR INTEGER)
BEGIN
 	SET @COUNTMESAS = 0;
    
    -- Verificando se existem contas já reservadas nesta mesa
	SELECT COUNT(*) INTO @COUNTMESAS FROM CONTA WHERE CONTA.NR_MESA = NR_MESA 
												AND CONTA.VALOR_RECEBIDO IS NULL
                                                AND CONTA.DATA_CONTA = DATE(NOW());
                                                
	IF @COUNTMESAS > 0
		THEN SET VALOR = 0;
	ELSE
		SET VALOR = 1;
	END IF;
    
END//
DELIMITER ;


-- TESTE ABRIR_CONTA -- 

-- 1) Tentando abrir uma conta em uma mesa já reservada
 -- SET @TEST = 0;
 
-- CALL ABRIR_CONTA (1, @TEST);

--  SELECT @TEST;

-- 2) Abrindo uma conta em uma mesa disponível
-- SET @TEST = 0;
 
-- CALL ABRIR_CONTA (2, @TEST);

-- SELECT @TEST;

-- FIM DOS TESTES DO ABRIR_CONTA --

DELIMITER //
CREATE PROCEDURE ABRIR_CONTA (IN NR_MESA INTEGER, OUT NR_CONTA INTEGER)
BEGIN
   SET @AVAILABLE = 0;
   SET @NRMESA = 0;
   
   CALL CHECK_DISPONIBILIDADE_MESA(NR_MESA, @AVAILABLE);
   
   -- Verificando se existe a mesa cadastrada na tabela mesa
   SELECT MESA.NR_MESA INTO @NRMESA FROM MESA WHERE MESA.NR_MESA = NR_MESA;
    
   -- Mesa não disponível
   IF @AVAILABLE = 0 OR @NRMESA = 0
	   THEN SET NR_CONTA = -1;
   -- Mesa disponível
   ELSE
   	BEGIN
		-- Reseta inicialmente o auto_increment
		ALTER TABLE CONTA AUTO_INCREMENT = 1;
        
        -- Então insere uma nova Conta
		INSERT INTO CONTA(DATA_CONTA, VALOR_RECEBIDO, NR_MESA) VALUES(DATE(NOW()), NULL, NR_MESA);
        
        -- Retorna o último inteiro criado para o AUTO_INCREMENT da tabela Conta
        SET NR_CONTA = LAST_INSERT_ID();
    END;
    
   END IF;
END//
DELIMITER ;

-- TESTE EXIBIR_CARDAPIO --

-- 1)Exibindo o cardápio com os pratos já cadastrados
-- CALL EXIBIR_CARDAPIO();

-- FIM DOS TESTES DO EXIBIR_CARDAPIO --

DELIMITER //
CREATE PROCEDURE EXIBIR_CARDAPIO ()
BEGIN
   SELECT ID_PRATO AS 'Número do prato', 
		  NOME AS 'Nome do prato',
          PRECO_UNITARIO AS 'Preço unitário' 
          FROM PRATO;
END//
DELIMITER ;

-- TESTE EFETUAR_PEDIDO --

-- 1) Inserção normal de um pedido com o garçom 3 -> Cria um novo registro
-- CALL EFETUAR_PEDIDO(1, 3, 5, 3);

-- 2) Inserção do mesmo prato com o mesmo garçom (Prato ainda não entregue) -> Soma na quantidade
-- CALL EFETUAR_PEDIDO(1, 3, 2, 3);

-- 3) Inserção do mesmo prato porém com garçom diferente -> Cria um novo registro
-- CALL EFETUAR_PEDIDO(1, 3, 5, 2);

-- FIM DOS TESTES DO EFETUAR_PEDIDO -- 

DELIMITER //
CREATE PROCEDURE EFETUAR_PEDIDO (IN NR_MESA INTEGER, IN NR_PRATO INTEGER, IN QTD INTEGER, IN ID_GARCON INTEGER)
BEGIN
	SET @NRCONTA = 0;
    SET @NRGARCON = 0;
    SET @NRPRATO = 0;
    SET @NRMESA = 0;
    
    -- Verificando se existe alguma conta nesta mesa
	SELECT CONT.NR_CONTA INTO @NRCONTA FROM CONTA CONT WHERE CONT.NR_MESA = NR_MESA
													   AND CONT.DATA_CONTA = DATE(NOW())
                                                       AND CONT.VALOR_RECEBIDO IS NULL;
  
	-- Verificando a existência do garçom
    SELECT GAR.NR_GARCON INTO @NRGARCON FROM GARCON GAR WHERE GAR.NR_GARCON = ID_GARCON;
    
    -- Verificando a existência do prato
	SELECT PRAT.ID_PRATO INTO @NRPRATO FROM PRATO PRAT WHERE PRAT.ID_PRATO = NR_PRATO;
    
	-- Caso a mesa não esteja disponível e exista uma conta reservada para esta mesa			
	IF @NRGARCON > 0 AND @NRPRATO > 0
		THEN 
        BEGIN
			-- Caso não exista uma conta nesta mesa, então a criamos
			IF @NRCONTA = 0
				THEN CALL ABRIR_CONTA(NR_MESA, @NRCONTA);
			END IF;
    
			-- Buscando o preço do prato em questão
            SET @PRECUNIT = 0;
            SELECT PRAT.PRECO_UNITARIO INTO @PRECUNIT FROM PRATO PRAT WHERE PRAT.ID_PRATO = NR_PRATO;
            
			-- Caso já existam pedidos cadastrados com o mesmo id do prato 
			-- verificamos se este garçom já realizou algum pedido com este prato
			SET @GARCONCOUNT = 0;
			SELECT COUNT(*) INTO @GARCONCOUNT FROM PEDIDOS WHERE PEDIDOS.NR_CONTA = @NRCONTA
														   AND PEDIDOS.ID_GARCON = ID_GARCON
														   AND PEDIDOS.NR_PRATO = NR_PRATO
                                                           AND PEDIDOS.ENTREGUE = FALSE;
			
			-- Caso exista, apenas realizamos um update na quantidade
			IF @GARCONCOUNT > 0
				THEN UPDATE PEDIDOS PED SET PED.QUANTIDADE = PED.QUANTIDADE + QTD WHERE PED.NR_PRATO = NR_PRATO
																				  AND PED.ID_GARCON = ID_GARCON
																				  AND PED.NR_CONTA = @NRCONTA
                                                                                  AND PED.ENTREGUE = FALSE;
            -- Caso não exista, inserimos um novo registro                                                                      
            ELSE
				BEGIN
				-- Buscamos um novo número de sequência
				SET @NRSEQ = 0;
				SELECT (COUNT(*) + 1) INTO @NRSEQ FROM PEDIDOS WHERE PEDIDOS.NR_CONTA = @NRCONTA;
				
				-- Inserimos um novo pedido
				INSERT INTO PEDIDOS VALUES(@NRCONTA, NR_PRATO, @NRSEQ, @PRECUNIT, QTD, ID_GARCON, NULL, FALSE);
                END;
			END IF;
            
        END;
	END IF;
END//
DELIMITER ;

-- TESTE ENTREGAR_PEDIDO --

-- 1) Entregando um pedido que possui mais de dois registros do mesmo prato com a mesma quantidade com garçom diferente (Número sequencial)
-- CALL ENTREGAR_PEDIDO(1, 5);

-- 2) Entregando o próximo pedido da mesa, do mesmo prato (5)
-- CALL ENTREGAR_PEDIDO(1, 5);

-- 3) Entregando o último da mesa 1 
-- CALL ENTREGAR_PEDIDO(1, 2);

-- FIM DOS TESTES DO ENTREGAR_PEDIDO --

DELIMITER //
CREATE PROCEDURE ENTREGAR_PEDIDO (IN NR_MESA INTEGER, IN NR_PRATO INTEGER)
BEGIN
	SET @AVAILABLE = 0;
    SET @NRPRATO = 0;
    SET @NRMESA = 0;
    SET @NRCONTA = 0;
    
    -- Verificando se a mesa não está disponível
    CALL CHECK_DISPONIBILIDADE_MESA(NR_MESA, @AVAILABLE);
    
	-- Verificando se existe a mesa cadastrada na tabela mesa
	SELECT MESA.NR_MESA INTO @NRMESA FROM MESA WHERE MESA.NR_MESA = NR_MESA;
    
    -- Verificando a existência do prato
	SELECT PRAT.ID_PRATO INTO @NRPRATO FROM PRATO PRAT WHERE PRAT.ID_PRATO = NR_PRATO;
    
    -- Pegando a conta que está na mesa
    SELECT CONT.NR_CONTA INTO @NRCONTA FROM CONTA CONT WHERE CONT.NR_MESA = NR_MESA
										               AND CONT.VALOR_RECEBIDO IS NULL
                                                       AND CONT.DATA_CONTA = DATE(NOW());
    
	-- Caso a mesa não esteja disponível e exista uma conta reservada para esta mesa			
	IF @AVAILABLE = 0 AND @NRPRATO > 0 AND @NRMESA > 0 AND @NRCONTA > 0
		THEN
        BEGIN
			 SET @COUNTPEDIDOS = 0;
             
             -- Verificando a quantidade de pedidos para o prato exigido
             SELECT COUNT(*) INTO @COUNTPEDIDOS FROM PEDIDOS PED WHERE PED.NR_PRATO = NR_PRATO
																 AND PED.ENTREGUE = FALSE
																 AND PED.NR_CONTA = @NRCONTA;
             
             -- Caso exista mais de um pedido com o mesmo prato na mesa
             -- consideramos a menor quantidade e o menor número sequencial
             IF @COUNTPEDIDOS > 1 THEN
                BEGIN
					SET @MINQTD = 0;
                    SET @MINSEQ = 0;
                    
                    -- Pegando a menor quantidade dos registros de pedido deste prato
                    SELECT MIN(PED.QUANTIDADE) INTO @MINQTD FROM PEDIDOS PED WHERE PED.NR_PRATO = NR_PRATO
																			 AND PED.NR_CONTA = @NRCONTA
																			 AND PED.ENTREGUE = FALSE;
                                                                             
                    -- Pegando o menor número sequencial (caso exista mais de uma quantidade mínima)                                                         
                	SELECT MIN(PED.NR_SEQUENC) INTO @MINSEQ FROM PEDIDOS PED WHERE PED.NR_PRATO = NR_PRATO
																		     AND PED.NR_CONTA = @NRCONTA
																	         AND PED.ENTREGUE = FALSE;
                                                                             
                	UPDATE PEDIDOS SET ENTREGUE = TRUE WHERE PEDIDOS.NR_PRATO = NR_PRATO
													   AND PEDIDOS.NR_CONTA = @NRCONTA
													   AND PEDIDOS.ENTREGUE = FALSE
                                                       AND PEDIDOS.NR_SEQUENC = @MINSEQ
                                                       AND PEDIDOS.QUANTIDADE = @MINQTD;
                END;
                
			 ELSE
				UPDATE PEDIDOS SET PEDIDOS.ENTREGUE = TRUE WHERE PEDIDOS.NR_PRATO = NR_PRATO
														   AND PEDIDOS.ENTREGUE = FALSE
														   AND PEDIDOS.NR_CONTA = @NRCONTA;
             END IF;
        END;
	END IF;
   
   
END//
DELIMITER ;

-- TESTE CONSULTA_CONTA --

-- 1) Consultando a conta da mesa 1
-- CALL CONSULTA_CONTA(1);

-- FIM DOS TESTES DO CONSULTA_CONTA --

DELIMITER //
CREATE PROCEDURE CONSULTA_CONTA (IN NR_MESA INTEGER)
BEGIN
   	SELECT PED.NR_CONTA AS 'Número da conta',
		   PED.NR_PRATO AS 'Número do prato',
           PRAT.NOME AS 'Nome do prato',
		   PED.QUANTIDADE AS 'Quantidade',
		   PED.PREC_UNIT AS 'Preço unitário',
           CAST((PED.PREC_UNIT * PED.QUANTIDADE) AS DECIMAL(10,2)) AS 'Total a pagar'
           FROM PEDIDOS AS PED
				INNER JOIN PRATO PRAT 
                ON PRAT.ID_PRATO = PED.NR_PRATO
		        AND EXISTS(SELECT * FROM CONTA WHERE CONTA.NR_MESA = NR_MESA 
											   AND PED.NR_CONTA = CONTA.NR_CONTA
                                               AND CONTA.VALOR_RECEBIDO IS NULL)
		   ORDER BY PED.NR_CONTA;
END//
DELIMITER ;

-- TESTE EXIBIR_COMISSOES_DEVIDAS --

-- 1) Exibindo as comissões do garçom 1
-- CALL EXIBIR_COMISSOES_DEVIDAS(1);

-- FIM DOS TESTES DO EXIBIR_COMISSOES_DEVIDAS -- 

DELIMITER //
CREATE PROCEDURE EXIBIR_COMISSOES_DEVIDAS (IN NR_GARCON INTEGER)
BEGIN
	SELECT GAR.NR_GARCON AS 'Número do garçom', 
           GAR.NOME AS 'Nome do garçom',
		   PED.NR_CONTA 'Número da conta'
           FROM GARCON AS GAR
		   INNER JOIN PEDIDOS PED ON PED.ID_GARCON = NR_GARCON 
								  AND PED.ID_GARCON = GAR.NR_GARCON 
							      AND PED.DATA_PAG_COMISSAO IS NULL;
END//
DELIMITER ;

-- TESTE PAGAR_COMISSOES --

-- 1)Efeutando o pagamento do garçom 1
-- CALL PAGAR_COMISSOES(1);

-- FIM DOS TESTES DO PAGAR_COMISSOES --

DELIMITER //
CREATE PROCEDURE PAGAR_COMISSOES (IN NR_GARCON INTEGER)
BEGIN
	UPDATE PEDIDOS PED SET PED.DATA_PAG_COMISSAO = DATE(NOW()) 
					   WHERE PED.ID_GARCON = NR_GARCON 
			           AND PED.DATA_PAG_COMISSAO IS NULL;
END//
DELIMITER ;

-- TESTE FECHAR_CONTA --

-- 1) Fechar conta da mesa 1
-- CALL FECHAR_CONTA(1);

-- FIM DOS TESTES FECHAR_CONTA --

DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `FECHAR_CONTA`(IN NR_MESA INTEGER)
BEGIN
	SET @AVAILABLE = 0;
	SET @NRMESA = 0;
    
    -- Mesa não disponível (possui uma conta)
    CALL CHECK_DISPONIBILIDADE_MESA(NR_MESA, @AVAILABLE);
    
    -- Mesa existente
    SELECT MESA.NR_MESA INTO @NRMESA FROM MESA WHERE MESA.NR_MESA = NR_MESA;
    
    IF @AVAILABLE = 0 AND @NRMESA > 0 THEN
		BEGIN
			SET @VALORPAGO = 0;
            SET @NRCONTA = 0;
            
            -- Valor total a ser pago na mesa
            SELECT SUM(PED.QUANTIDADE * PED.PREC_UNIT) INTO @VALORPAGO FROM PEDIDOS PED INNER JOIN CONTA ON CONTA.NR_MESA = NR_MESA
																						AND PED.NR_CONTA = CONTA.NR_CONTA
																						-- AND PED.ENTREGUE = TRUE
																						AND CONTA.VALOR_RECEBIDO IS NULL;
            
            IF @VALORPAGO = "" OR @VALORPAGO IS NULL
				THEN SET @VALORPAGO = 0;
			END IF;
                                                                 
			-- Adicionando o valor a ser pago																					
			UPDATE CONTA SET CONTA.VALOR_RECEBIDO = @VALORPAGO WHERE CONTA.NR_MESA = NR_MESA;
		END;
	END IF;
END//
DELIMITER ;