output "service_url" {
  value = google_cloud_run_service.user-management-service.status[0].url
}