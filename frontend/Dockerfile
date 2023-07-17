FROM node:18-alpine3.16

WORKDIR /app/front

COPY package*.json ./

RUN npm install

RUN npm install @mui/material @emotion/react @emotion/styled

RUN npm install @mui/material @mui/styled-engine-sc styled-components

RUN npm install @mui/icons-material

COPY . .

EXPOSE 3000

CMD ["npm", "run", "start"]