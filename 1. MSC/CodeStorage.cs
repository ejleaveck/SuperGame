   float GetPlayerMoveDirection()
   {
       float direction = 0f;

        leftButtonPressed = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        rightButtonPressed = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

     
      if (rightButtonPressed)
       {
           direction = 1f;
       }
       else if (leftButtonPressed)
       {
           direction = -1f;
       }
       else
       {
           direction = 0f;
       }

       return direction;

   }