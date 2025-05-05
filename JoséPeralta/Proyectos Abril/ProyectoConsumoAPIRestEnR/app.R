# --------------------------
# Consumo de DummyJSON API en R con Shiny
# --------------------------

# 1. Instalación de paquetes necesarios (ejecutar una sola vez)
# install.packages(c("httr", "jsonlite", "dplyr", "ggplot2", "shiny", "DT", 
#                   "shinydashboard", "shinydashboardPlus", "shinyjs", "shinyWidgets", 
#                   "shinyBS", "shinyalert", "shinyauthr", "shinycssloaders", 
#                   "leaflet", "shinyTime", "sodium", "openxlsx", "flextable", 
#                   "magick", "shinyFiles", "openssl"))

# 2. Cargar las bibliotecas necesarias
library(shiny)
library(shinyjs)
library(shinydashboard)
library(dplyr)
library(httr)
library(jsonlite)
library(ggplot2)
library(DT)

# 3. Definir la interfaz de usuario (UI)
ui <- fluidPage(
  useShinyjs(),
  titlePanel("Consumo de DummyJSON API"),
  
  sidebarLayout(
    sidebarPanel(
      numericInput("product_limit", "Número de productos:", value = 10, min = 1, max = 100),
      textInput("search_query", "Buscar productos:", value = ""),
      actionButton("search_button", "Buscar", class = "btn-primary")
    ),
    
    mainPanel(
      tabsetPanel(
        tabPanel("Productos", 
                 br(),
                 DT::dataTableOutput("products_table"),
                 br(),
                 plotOutput("products_chart", height = "400px")),
        tabPanel("Usuarios", 
                 br()),
        tabPanel("Detalles del Producto",
                 br(), 
                 uiOutput("product_details"))
      )
    )
  )
)

# 4. Definir el servidor
server <- function(input, output, session) {
  
  # Configurar la URL base de la API
  base_url <- "https://dummyjson.com"
  
  # Función para obtener datos de la API
  get_data_from_api <- function(endpoint) {
    url <- paste0(base_url, endpoint)
    response <- GET(url)
    
    # Verificar si la respuesta fue exitosa (código 200)
    if (status_code(response) == 200) {
      # Convertir el contenido JSON a un objeto R
      content <- content(response, "text")
      data <- fromJSON(content)
      return(data)
    } else {
      warning(paste("Error en la solicitud:", status_code(response)))
      return(NULL)
    }
  }
  
  # Reactivos para almacenar datos
  products_data <- reactiveVal(NULL)
  users_data <- reactiveVal(NULL)
  selected_product <- reactiveVal(NULL)
  
  # Cargar productos iniciales
  observe({
    products <- get_data_from_api(paste0("/products?limit=", input$product_limit))
    if (!is.null(products)) {
      products_data(products$products)
    }
  })
  
  # Buscar productos cuando se hace clic en el botón de búsqueda
  observeEvent(input$search_button, {
    query <- input$search_query
    if (nchar(query) > 0) {
      search_results <- get_data_from_api(paste0("/products/search?q=", query))
      if (!is.null(search_results)) {
        products_data(search_results$products)
      }
    } else {
      # Si la búsqueda está vacía, cargar productos por defecto
      products <- get_data_from_api(paste0("/products?limit=", input$product_limit))
      if (!is.null(products)) {
        products_data(products$products)
      }
    }
  })
  
  # Tabla de productos
  # output$products_table <- DT::renderDataTable({
  #   req(products_data())
  #   df <- products_data() %>%
  #     select(id, title, price, discountPercentage, rating, brand, category)
  #   
  #   DT::datatable(
  #     df,
  #     options = list(pageLength = 5),
  #     selection = 'single'
  #   )
  # })

  # output$products_table <- renderDT({
  #   
  #   req(products_data())
  #   
  #   # Filtrar y seleccionar columnas relevantes
  #   # Usamos %>% para encadenar operaciones
  #   # Y select() para elegir las columnas que queremos mostrar
  #   df <- products_data() %>%
  #     select(id, title, price, discountPercentage, rating, brand, category)   
  #   
  #   datatable(df, selection = 'single', class = c('table-striped', 'table-bordered'),
  #             extensions = c('Responsive'), colnames = c('ID', 'TITLE', 'PRICE', 'DISCOUNT', 'RATING', 'BRAND', 'CATEGORY'),
  #             options = list(
  #               pageLength = 5, 
  #               lengthMenu = list(c(5, 10, 15, 25, 50), c('5', '10', '15', '25', '50')),
  #               autoWidth = FALSE, 
  #               searching = TRUE, 
  #               dom = 'Blfrtip',  # Añadida la 'l' para mostrar el selector de longitud
  #               language = list(url = '//cdn.datatables.net/plug-ins/1.10.11/i18n/Spanish.json')
  #             ), 
  #             rownames = FALSE, 
  #             editable = FALSE)
  # })
  
  output$products_table <- renderDT({
    req(products_data())
    
    df <- products_data() %>%
      select(id, title, price, discountPercentage, rating, brand, category)   
    
    datatable(df, 
              selection = 'single', 
              class = c('table-striped', 'table-bordered'),
              extensions = c('Responsive', 'Buttons'), 
              colnames = c('ID', 'TÍTULO', 'PRECIO', 'DESCUENTO', 'RATING', 'MARCA', 'CATEGORÍA'),
              options = list(
                pageLength = 10, 
                lengthMenu = list(c(5, 10, 15, 25, 50), c('5', '10', '15', '25', '50')),
                autoWidth = FALSE, 
                searching = TRUE, 
                dom = '<"row"<"col-sm-4"B><"col-sm-4"l><"col-sm-4"f>><"row"<"col-sm-12"tr>><"row"<"col-sm-5"i><"col-sm-7"p>>',
                buttons = list(
                  'copy', 'csv', 'excel', 'pdf', 'print',
                  list(
                    extend = 'colvis',
                    text = 'Columnas'
                  )
                ),
                language = list(url = '//cdn.datatables.net/plug-ins/1.10.11/i18n/Spanish.json')
              ), 
              rownames = FALSE
    )
  })
  
  
  # Observar selección de producto en la tabla
  observeEvent(input$products_table_rows_selected, {
    req(products_data())
    selected_row <- input$products_table_rows_selected
    if (length(selected_row) > 0) {
      product_id <- products_data()[selected_row, "id"]
      product_details <- get_data_from_api(paste0("/products/", product_id))
      selected_product(product_details)
    }
  })
  
  # Mostrar detalles del producto seleccionado
  output$product_details <- renderUI({
    req(selected_product())
    product <- selected_product()
    
    tagList(
      h3(product$title),
      div(
        style = "display: flex; margin-bottom: 20px;",
        div(
          style = "flex: 1;",
          img(src = product$thumbnail, width = "250px", style = "border-radius: 8px;")
        ),
        div(
          style = "flex: 2; padding-left: 20px;",
          p(strong("Descripción: "), product$description),
          p(strong("Precio: "), paste0("$", product$price)),
          p(strong("Descuento: "), paste0(product$discountPercentage, "%")),
          p(strong("Valoración: "), product$rating, " / 5"),
          p(strong("Marca: "), product$brand),
          p(strong("Categoría: "), product$category),
          p(strong("Stock: "), product$stock)
        )
      ),
      h4("Imágenes del producto"),
      div(
        style = "display: flex; flex-wrap: wrap; gap: 10px;",
        lapply(product$images, function(img_url) {
          img(src = img_url, width = "150px", style = "border-radius: 4px;")
        })
      )
    )
  })
  
  # Gráfico de productos (10 más caros)
  output$products_chart <- renderPlot({
    req(products_data())
    top_products <- products_data() %>%
      arrange(desc(price)) %>%
      head(10)
    
    ggplot(top_products, aes(x = reorder(title, price), y = price)) +
      geom_bar(stat = "identity", fill = "steelblue") +
      coord_flip() +
      labs(title = "10 productos más caros",
           x = "Producto",
           y = "Precio ($)") +
      theme_minimal()
  })
  

  
}

# 5. Ejecutar la aplicación
shinyApp(ui, server)