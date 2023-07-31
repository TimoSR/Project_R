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
  default     = "128Mi"
}