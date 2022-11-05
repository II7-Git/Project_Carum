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
import { useEffect, useState } from "react";
import { fetchProfile } from "apis/user";
import UnityCarum from "../../components/unity/UnityCarum";

function Main() {
  const location = useLocation();
  const [user, setUser] = useState(null);

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
    setUser(userInfo);
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
        <UnityCarum />
      </div>
      <div className={location.pathname === "/main" ? styles.contentBox : null}>
        <Routes>
          <Route path=":state" element={<DiaryWrite />} />
          <Route path="diary/:id" element={<Diary />} />
          <Route path="calendar" element={<CalendarDiary />} />
          <Route path="room" element={<Room />} />
          <Route path="shop" element={<Shop />} />
          <Route path="profile" element={<Profile />} />
          <Route path="yearly-pet" element={<YearlyPet />} />
          <Route path="monthly-pet" element={<MonthlyPet />} />
        </Routes>
        {location.pathname === "/main" ? <Menu user={user} /> : null}
      </div>
    </div>
  );
}

export default Main;
