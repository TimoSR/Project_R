output "service_url" {
  value = google_cloud_run_service.game-cms-service.status[0].url
}