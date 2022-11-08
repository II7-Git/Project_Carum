import Header from "../../components/Header/Header";
import styles from "./Main.module.css";
import { useLocation, Routes, Route } from "react-router-dom";
import Diary from "../Diary/Diary";
import DiaryWrite from "../Diary/DiaryWrite";
import Room from "../Room/Room";
import Shop from "../Shop/Shop";
import MonthlyPet from "pages/Pet/MonthlyPet/MonthlyPet";
import YearlyPet from "pages/Pet/YearlyPet/YearlyPet";
import Profile from "../Profile/Profile";
import CalendarDiary from "../Diary/CalendarDiary/CalendarDiary";
import Menu from "./Menu";
import { useEffect, useState, useCallback } from "react";
import { fetchProfile } from "apis/user";
import UnityCarum from "../../components/unity/UnityCarum";
import { setNowRoomId } from "stores/slices/room";
import { useAppDispatch } from "stores/store";
import { setUserInfo } from "stores/slices/user";
import React, { useRef } from "react";

function Main() {
  const location = useLocation();
  const childRef = useRef(null);

  const enterCloseUp = () => {
    childRef.current.enterCloseUp();
  };
  const exitCloseUp = () => {
    childRef.current.exitCloseUp();
  };

  const [user, setUser] = useState(null);

  const dispatch = useAppDispatch();

  const changeRoom = useCallback(
    (id) => {
      dispatch(setNowRoomId(id));
    },
    [dispatch]
  );

  const handleUserInfo = useCallback(
    (userInfo) => {
      dispatch(setUserInfo(userInfo));
    },
    [dispatch]
  );

  const fetchProfileSuccess = (res) => {
    console.log(res);
    const userInfo = {
      nickname: res.data.nickName,
      id: res.data.userId,
      birth: res.data.birth,
      phone: res.data.phone,
      money: res.data.money,
      mainRoom: res.data.mainRoom,
      todayDiary: res.data.todayDiary,
    };
    changeRoom(res.data.mainRoom.id);
    setUser(userInfo);
    handleUserInfo(userInfo);
  };

  const fetchProfileFail = (err) => {
    console.log(err);
  };

  useEffect(() => {
    fetchProfile(fetchProfileSuccess, fetchProfileFail);
  }, []);

  return (
    <div className={styles.container}>
      <div className={styles.unity}>
        <UnityCarum ref={childRef} />
      </div>
      <div className={location.pathname === "/main" ? styles.contentBox : null}>
        <Routes>
          <Route
            path=":state"
            element={
              <DiaryWrite
                enterCloseUp={enterCloseUp}
                exitCloseUp={exitCloseUp}
              />
            }
          />
          <Route path="diary/:id" element={<Diary unityRef={childRef} />} />
          <Route path="calendar" element={<CalendarDiary />} />
          <Route path="room" element={<Room />} />
          <Route path="shop" element={<Shop />} />
          <Route path="profile" element={<Profile />} />
          <Route path="yearly-pet/:year" element={<YearlyPet />} />
          <Route path="monthly-pet/:year/:month" element={<MonthlyPet />} />
        </Routes>
        {location.pathname === "/main" ? <Menu user={user} /> : null}
      </div>
    </div>
  );
}

export default Main;
