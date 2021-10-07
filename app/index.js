import React, {Component} from 'react';
import { KeyboardAvoidingView, LogBox } from 'react-native';
import MainNavigation from './navigation/mainNavigation';
import { enableScreens } from 'react-native-screens';

enableScreens();
LogBox.ignoreAllLogs();

class App extends Component {
  render() {
    return (
      <KeyboardAvoidingView style={{flex: 1}}>
        <MainNavigation/>
      </KeyboardAvoidingView>
    );
  }
}
export default App;
