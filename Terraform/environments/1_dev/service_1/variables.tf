variable "env_vars" {
  description = "Environment variables for the Cloud Run service"
  type = map(string)
  default = {    

    ########################################################################################
    # HOST_URL should be URL without ending '/'
    ########################################################################################
    "HOST_URL"                    = "https://x-service-s4qgcf5egq-ew.a.run.app"
    "SERVICE_NAME"                = "x-service"
    "ENVIRONMENT"                 = "Development"
    "GOOGLE_CLOUD_PROJECT"        = "project-r-393911"

    ########################################################################################
    
  }
}

variable "project" {
  default     = "project-r-393911"
}

variable "region" {
  description = "The region where the Cloud Run service will be deployed."
  default     = "europe-west1" # Replace with your preferred region.
}

variable "container_image" {
  description = "The container image to deploy."
  default     = "docker.io/00tir2009/x_service_dev:latest" # Replace with your container image.
}

variable "cpu" {
  description = "CPU allocation for the Cloud Run service"
  type        = string
  default     = "1"
}

variable "memory" {
  description = "Memory allocation for the Cloud Run service"
  type        = string
  default     = "512Mi"
}