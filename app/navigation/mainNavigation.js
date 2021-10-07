import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import entryScreen from '../entryScreen/entryScreen';

const Stack = createStackNavigator();

const MainNavigation = () => {
 
  return (
    <NavigationContainer>
      <Stack.Navigator headerMode="none" screenOptions={{
        gestureEnabled:false
      }}>
        
      <Stack.Screen name="entryScreen" component={entryScreen} />

      </Stack.Navigator>
    </NavigationContainer>
  );
};

export default MainNavigation;
