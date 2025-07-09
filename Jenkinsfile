pipeline {
    agent any

    environment {
        SOLUTION_FILE = 'Weather.API.sln'
        DOTNET_IMAGE  = 'mcr.microsoft.com/dotnet/sdk:9.0'
    }

    stages {
        stage('Clone Repository') {
            steps {
                git branch: 'master', url: 'https://github.com/AleksejIgnatenko/Weather.API'
            }
        }

        stage('Restore') {
            steps {
                script {
                    docker.image(env.DOTNET_IMAGE).inside {
                        sh "dotnet restore ${env.SOLUTION_FILE}"
                    }
                }
            }
        }

        stage('Build') {
            steps {
                script {
                    docker.image(env.DOTNET_IMAGE).inside {
                        sh "dotnet build ${env.SOLUTION_FILE} --configuration Release"
                    }
                }
            }
        }

        stage('Test') {
            steps {
                script {
                    docker.image(env.DOTNET_IMAGE).inside {
                        sh "dotnet test ${env.SOLUTION_FILE} --configuration Release"
                    }
                }
            }
        }
    }
}
