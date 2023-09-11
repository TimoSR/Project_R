variable "env_vars" {
  description = "Environment variables for the Cloud Run service"
  type = map(string)
  default = {

    "SERVICE_NAME"                = "x-service"
    "ENVIRONMENT"                 = "Development"
    "GOOGLE_CLOUD_PROJECT"        = "project-r-393911"

    ########################################################################################
    "TOPIC_PRODUCT_UPDATES"       = "Product"
    "TOPIC_ORDER_UPDATES"         = "OrderUpdated"

    # Your subscriptions and endpoints need to go 1:1
    ########################################################################################
    "SUBSCRIBE_PRODUCT_UPDATES"   = "x-service-Product"
    
    ########################################################################################
    "ENDPOINT_PRODUCT_UPDATES"    = "https://x-service-s4qgcf5egq-ew.a.run.app/api/PubSub/Subscription1"
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