variable "env_vars" {
  description = "Environment variables for the Cloud Run service"
  type = map(string)
  default = {
    "SERVICE_NAME"                = "x-service"
    "ENVIRONMENT"                 = "Production"
    "TOPIC_PRODUCT_UPDATES"       = "product-updates-v1"
    "TOPIC_ORDER_UPDATES"         = "order-updates-v1"
    "SUBSCRIBE_PRODUCT_UPDATES"   = "x-service-product-updates-v1"
    "SUBSCRIBE_ORDER_UPDATES"     = "x-service-order-updates-v1"
    "ENDPOINT_PRODUCT_UPDATES"    = "https://riftgate.ngrok.io/api/PubSub/Subscription1"
    "ENDPOINT_ORDER_UPDATES"      = "https://riftgate.ngrok.io/api/PubSub/Subscription2"
    "MONGODB_DB"                  = "Production"
    "GOOGLE_CLOUD_PROJECT"        = "project-r-393911"
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
  default     = "256Mi"
}