<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Database\Eloquent\Model;

class Sach extends Model
{
    protected $table = 'sach';
    public function ChiTietDonHang()
    {
        return $this->hasMany(ChiTietDonHang::class, 'sach_id', 'id');
    }
    protected $fillable = [
        'danhmuc_id',
        'nhaxuatban_id',
        'tieudesach',
        'tacgia_id',
        'mota',
        'giasach',
        'giasale',
        'trangthai',
        'soluong',
        'hinhanh',
    ];

    public function DanhMuc()
    {
        return $this->belongsTo(DanhMuc::class);
    }

    public function NhaXuatBan()
    {
        return $this->belongsTo(NhaXuatBan::class);
    }
}
