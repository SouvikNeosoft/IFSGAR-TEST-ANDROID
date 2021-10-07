import React, { useState } from 'react';
import { Button, SafeAreaView, Text, View } from 'react-native';
import styles from "./style";
import UnityView, { UnityModule } from '@asmadsen/react-native-unity-view';

const entryScreen = ({navigation}) => {

  const [count, setClickCount] = useState(0)
  console.log(count)
  const onUnityMessage = (hander) => {
    console.log({hander})
  }

  const onClick = () => {
      UnityModule.postMessageToUnityManager({
                                                name: 'ToggleRotate',
                                                data: '',
                                                callBack: (data) => {
                                                    Alert.alert('Tip', JSON.stringify(data))
                                                }
                                            });
  }

    
    return (
      <SafeAreaView style={styles.container}>
        <Text>Entry Screen</Text>

          <View style={{ flex: 1 }}>
            <UnityView
                style={{ flex: 1 }}
                onMessage={onUnityMessage}
                onUnityMessage={onUnityMessage}
            />
          </View>
        <Button style={{ width: '100%' }} title="Toggle rotation" onPress={onClick}/>

      </SafeAreaView>
    );
};
export default entryScreen;