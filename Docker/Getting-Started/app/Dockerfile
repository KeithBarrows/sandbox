FROM node:18-alpine
WORKDIR /app
COPY . .
RUN yarn install --production
CMD ["node", "src/index.js"]

#-------------------------------------------------------------------------------
# http://localhost/tutorial/our-application/
# STEP: Our Application
#-------------------------------------------------------------------------------
# docker build -t getting-started .
# docker run -dp 3000:3000 getting-started
#-------------------------------------------------------------------------------
