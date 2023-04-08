# Descripción del proyecto

Este proyecto consiste en la implementación de un UserControl en C# para representar una batería. La batería se puede cargar o descargarse arrastrando el ratón por el control. Además, se pueden establecer diferentes colores para la batería en función del nivel de carga, si está cargando o si está agotada.

## Cómo usar

Para utilizar el control, simplemente agregue el archivo Bateria.cs y Bateria.Designer.cs a su proyecto. Puede utilizar el control en su formulario de la misma manera que cualquier otro control.

## Propiedades

- ChargeLevel: Controla el nivel de carga de la batería
- IsDepleted: Controla el color de la batería cuando está agotada
- IsCharging: Controla el color de la batería cuando está cargando

## Eventos

- LevelChanged: Se dispara cuando cambia el nivel de carga de la batería

## Ejemplo de uso

```
private void Form1_Load(object sender, EventArgs e)
{
    Bateria bateria = new Bateria();
    bateria.ChargeLevel = 50;
    bateria.IsCharging = true;
    this.Controls.Add(bateria);
}

```
