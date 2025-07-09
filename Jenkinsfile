pipeline {
    agent any

    environment {
        SOLUTION_FILE = 'Weather.API.sln'
    }

    stages {
        stage('Clone Repository') {
            steps {
                git branch: 'master', url: 'https://github.com/AleksejIgnatenko/Weather.API '
            }
        }

        stage('Restore') {
            steps {
                sh "dotnet restore ${env.SOLUTION_FILE}"
            }
        }

        stage('Build') {
            steps {
                sh "dotnet build ${env.SOLUTION_FILE} --configuration Release"
            }
        }

        stage('Test') {
            steps {
                sh "dotnet test ${env.SOLUTION_FILE} --configuration Release"
            }
        }

        stage('Pack') {
            steps {
                sh "dotnet pack ${env.SOLUTION_FILE} --configuration Release /p:PackageOutputPath=../artifacts"
            }
        }

        stage('Publish to Nexus') {
            steps {
                script {
                    def NEXUS_URL = "http://localhost:8081/service/rest/v1/components?repository=nuget-releases"
                    def NEXUS_USER = credentials('nexus-credentials-id').username
                    def NEXUS_PASS = credentials('nexus-credentials-id').password

                    def NUPKG_PATH = "artifacts/*.nupkg"

                    sh """
                        curl -u ${NEXUS_USER}:${NEXUS_PASS} -X POST "${NEXUS_URL}" \\
                          -H "Content-Type: multipart/form-data" \\
                          -F "maven2.groupId=" \\
                          -F "maven2.artifactId=" \\
                          -F "maven2.version=" \\
                          -F "maven2.asset1.extension=nupkg" \\
                          -F "maven2.asset1=@${NUPKG_PATH};type=application/octet-stream"
                    """
                }
            }
        }
    }
}
