TRUNCATE TABLE
area,
    department,
    gender,
    employee
RESTART IDENTITY CASCADE;

INSERT INTO area (area) VALUES ('東京'); 
INSERT INTO area (area) VALUES ('大阪'); 

INSERT INTO department (name, area_id) VALUES ('総務部', 1);
INSERT INTO department (name, area_id) VALUES ('経理部', 1);
INSERT INTO department (name, area_id) VALUES ('人事部', 1);
INSERT INTO department (name, area_id) VALUES ('開発部', 2);
INSERT INTO department (name, area_id) VALUES ('営業部', 2);

INSERT INTO gender (gender) VALUES ('男性'); 
INSERT INTO gender (gender) VALUES ('女性'); 
INSERT INTO gender (gender) VALUES ('その他'); 

INSERT INTO employee (name, dept_id, gender_id) VALUES ('田中太郎', 2, 1);
INSERT INTO employee (name, dept_id, gender_id) VALUES ('鈴木三郎', 1, 1);
INSERT INTO employee (name, dept_id, gender_id) VALUES ('佐藤花子', 4, 2);
INSERT INTO employee (name, dept_id, gender_id) VALUES ('中田彩子', 5, 2);
INSERT INTO employee (name, dept_id, gender_id) VALUES ('加藤圭太', 3, 1);
INSERT INTO employee (name, dept_id, gender_id) VALUES ('松本良太', 4, 1);
INSERT INTO employee (name, dept_id, gender_id) VALUES ('山下孝輔', 5, 1);
INSERT INTO employee (name, dept_id, gender_id) VALUES ('渡辺大輔', 4, 1);



